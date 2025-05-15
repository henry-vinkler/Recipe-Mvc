using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Facade;
using RecipeMvc.Domain;
using System.Security.Claims;
using RecipeMvc.Soft.Data;
using RecipeMvc.Facade.Recipe;

namespace RecipeMvc.Soft.Controllers;

[Authorize]
public class RecipesController : Controller
{
    private readonly ApplicationDbContext _db;

    public RecipesController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: Recipes
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? searchString)
    {
        var recipes = _db.Recipes
            .Include(r => r.Author)
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            recipes = recipes.Where(r => r.Title.Contains(searchString));
            ViewData["CurrentFilter"] = searchString;
        }

        var recipeViews = await recipes
            .Select(r => new RecipeView
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Tags = r.Tags,
                AuthorId = r.AuthorId,
                Ingredients = r.RecipeIngredients.Select(ri => new RecipeIngredientView
                {
                    IngredientId = ri.IngredientId,
                    IngredientName = ri.Ingredient.Name,
                    Quantity = ri.Quantity
                }).ToList()
            })
            .ToListAsync();

        // Calculate total calories without adding CaloriesPerUnit to RecipeIngredientView
        foreach (var recipe in recipeViews)
        {
            // Use the EF navigation properties loaded in Ingredients to get calories per unit
            recipe.Calories = recipe.Ingredients.Sum(ing =>
            {
                // Find the ingredient's calories per unit from the database or from cached data
                var ingredientCalories = _db.Ingredients
                    .Where(i => i.Id == ing.IngredientId)
                    .Select(i => i.Calories)
                    .FirstOrDefault();

                return ingredientCalories * ing.Quantity;
            });
        }

        return View(recipeViews);
    }


    // GET: Recipes/Create
    public async Task<IActionResult> Create()
    {
        await SetAvailableIngredientsAsync();
        return View(new RecipeView { Ingredients = new List<RecipeIngredientView>() });
    }

    // POST: Recipes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RecipeView model)
    {
        if (!ModelState.IsValid)
        {
            await SetAvailableIngredientsAsync();
            return View(model);
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var usernameClaim = User.FindFirst(ClaimTypes.Name);
        if (userIdClaim == null || usernameClaim == null)
        {
            ModelState.AddModelError("", "User not authenticated.");
            await SetAvailableIngredientsAsync();
            return View(model);
        }
        int authorId = int.Parse(userIdClaim.Value);

        string imagePath = null;
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            // Save to wwwroot/images/recipes (ensure this folder exists)
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "recipes");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            }
            imagePath = "/images/recipes/" + uniqueFileName;
        }

        var recipeData = new RecipeData
        {
            Title = model.Title,
            Description = model.Description,
            Calories = model.Calories,
            Tags = model.Tags,
            AuthorId = authorId,
            ImagePath = imagePath // Save the path or null
        };

        _db.Recipes.Add(recipeData);
        await _db.SaveChangesAsync();

        // Save ingredients (unchanged)
        if (model.Ingredients != null)
        {
            foreach (var ing in model.Ingredients)
            {
                if (ing.IngredientId > 0 && ing.Quantity > 0)
                {
                    var recipeIngredient = new RecipeIngredientData
                    {
                        RecipeId = recipeData.Id,
                        IngredientId = ing.IngredientId,
                        Quantity = ing.Quantity
                    };
                    _db.RecipeIngredients.Add(recipeIngredient);
                }
            }
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Recipes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var recipe = await _db.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null) return NotFound();

        var model = new RecipeView
        {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            Calories = recipe.Calories,
            Tags = recipe.Tags,
            AuthorId = recipe.AuthorId,
            Ingredients = recipe.RecipeIngredients
                .Select(ri => new RecipeIngredientView
                {
                    IngredientId = ri.IngredientId,
                    Quantity = ri.Quantity
                }).ToList()
        };

        await SetAvailableIngredientsAsync();
        return View(model);
    }

    // POST: Recipes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RecipeView model)
    {
        if (id != model.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            await SetAvailableIngredientsAsync();
            return View(model);
        }

        var recipe = await _db.Recipes.Include(r => r.RecipeIngredients).FirstOrDefaultAsync(r => r.Id == id);
        if (recipe == null) return NotFound();

        recipe.Title = model.Title;
        recipe.Description = model.Description;
        recipe.Calories = model.Calories;
        recipe.Tags = model.Tags;

        // Handle image update
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "recipes");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            }
            recipe.ImagePath = "/images/recipes/" + uniqueFileName;
        }

        // Update ingredients (unchanged)
        _db.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);
        if (model.Ingredients != null)
        {
            foreach (var ing in model.Ingredients)
            {
                if (ing.IngredientId > 0 && ing.Quantity > 0)
                {
                    _db.RecipeIngredients.Add(new RecipeIngredientData
                    {
                        RecipeId = recipe.Id,
                        IngredientId = ing.IngredientId,
                        Quantity = ing.Quantity
                    });
                }
            }
        }

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Recipes/Details/5
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var recipe = await _db.Recipes
            .Include(r => r.Author)
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null) return NotFound();

        var model = new RecipeView
        {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            Calories = recipe.Calories,
            Tags = recipe.Tags,
            AuthorId = recipe.AuthorId,
            Ingredients = recipe.RecipeIngredients
                .Select(ri => new RecipeIngredientView
                {
                    IngredientId = ri.IngredientId,
                    IngredientName = ri.Ingredient?.Name,
                    Quantity = ri.Quantity
                }).ToList()
        };

        return View(model);
    }

    // GET: Recipes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var recipe = await _db.Recipes.FindAsync(id);
        if (recipe == null) return NotFound();

        var model = new RecipeView
        {
            Id = recipe.Id,
            Title = recipe.Title
        };

        return View(model);
    }

    // POST: Recipes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var recipe = await _db.Recipes.Include(r => r.RecipeIngredients).FirstOrDefaultAsync(r => r.Id == id);
        if (recipe != null)
        {
            _db.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);
            _db.Recipes.Remove(recipe);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // Helper to set available ingredients for views
    private async Task SetAvailableIngredientsAsync()
    {
        ViewBag.AvailableIngredients = await _db.Ingredients
            .Select(i => new IngredientView { Id = i.Id, Name = i.Name, Unit = i.Unit })
            .ToListAsync();
    }
}