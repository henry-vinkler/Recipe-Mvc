using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using System.Security.Claims;
using RecipeMvc.Soft.Data;
using RecipeMvc.Facade.Recipe;

namespace RecipeMvc.Soft.Controllers;

[Authorize] public class RecipesController : Controller {
    private readonly ApplicationDbContext _db;
    private const byte pageSize = 6;
    public RecipesController(ApplicationDbContext db) {
        _db = db;
    }

    [AllowAnonymous] public async Task<IActionResult> Index(string? searchString, bool mine = false, int page = 1) {
        var recipes = _db.Recipes
            .Include(r => r.Author)
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString)) {
            var search = searchString.ToLower();
            recipes = recipes.Where(r => 
                r.Title.ToLower().Contains(search) ||
                r.Tags.ToLower().Contains(search));
            ViewData["CurrentFilter"] = searchString;
        }

        if (mine && User.Identity.IsAuthenticated) {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            recipes = recipes.Where(r => r.AuthorId == userId);
        }

        var totalCount = await recipes.CountAsync();
        var recipeViews = await recipes
            .OrderByDescending(r => r.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RecipeView {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Tags = r.Tags,
                AuthorId = r.AuthorId,
                Ingredients = r.RecipeIngredients.Select(ri => new RecipeIngredientView {
                    IngredientId = ri.IngredientId,
                    IngredientName = ri.Ingredient.Name,
                    Quantity = ri.Quantity,
                    Unit = ri.Ingredient.Unit
                }).ToList()
            }).ToListAsync();

        foreach (var recipe in recipeViews){
            recipe.Calories = recipe.Ingredients.Sum(ing => {
                var ingredientCalories = _db.Ingredients
                    .Where(i => i.Id == ing.IngredientId)
                    .Select(i => i.Calories)
                    .FirstOrDefault();
                return ingredientCalories * ing.Quantity;
            });
        }
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewData["CurrentFilter"] = searchString;
        return View(recipeViews);
    }

    public async Task<IActionResult> Create() {
        await SetAvailableIngredientsAsync();
        return View(new RecipeView { Ingredients = new List<RecipeIngredientView>() });
    }

    [HttpPost] [ValidateAntiForgeryToken] public async Task<IActionResult> Create(RecipeView model) {
        if (!ModelState.IsValid) {
            int userId = 0;
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out userId)) ModelState.AddModelError("", "User ID is invalid.");
            model.AuthorId = userId;
            await SetAvailableIngredientsAsync();
            return View(model);
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) {
            ModelState.AddModelError("", "User not authenticated.");
            await SetAvailableIngredientsAsync();
            return View(model);
        }
        int authorId = int.Parse(userIdClaim.Value);

        string imagePath = null;
        if (model.ImageFile != null && model.ImageFile.Length > 0) {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "recipes");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await model.ImageFile.CopyToAsync(stream);
            }
            imagePath = "/images/recipes/" + uniqueFileName;
        }

        var recipeData = new RecipeData {
            Title = model.Title,
            Description = model.Description,
            Tags = model.Tags,
            AuthorId = authorId,
            ImagePath = imagePath
        };

        if (model.Ingredients != null) {
            recipeData.Calories = model.Ingredients.Sum(ing => {
                var cal = _db.Ingredients.Where(i => i.Id == ing.IngredientId).Select(i => i.Calories).FirstOrDefault();
                return cal * ing.Quantity;
            });
        }
        else {
            recipeData.Calories = 0;
        }

        _db.Recipes.Add(recipeData);
        await _db.SaveChangesAsync();

        if (model.Ingredients != null) {
            var groupedIngredients = model.Ingredients
                .GroupBy(i => i.IngredientId)
                .Select(g => new RecipeIngredientData {
                    RecipeId = recipeData.Id,
                    IngredientId = g.Key,
                    Quantity = g.Sum(i => i.Quantity)
                });
            await _db.RecipeIngredients.AddRangeAsync(groupedIngredients);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id) {
        if (id == null) return NotFound();
        var recipe = await _db.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (recipe == null) return NotFound();
        var model = new RecipeView {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            Calories = recipe.Calories,
            Tags = recipe.Tags,
            Ingredients = recipe.RecipeIngredients
                .Select(ri => new RecipeIngredientView {
                    IngredientId = ri.IngredientId,
                    IngredientName = ri.Ingredient.Name,
                    Quantity = ri.Quantity,
                    Unit = ri.Ingredient.Unit
                }).ToList()
        };
        await SetAvailableIngredientsAsync();
        return View(model);
    }

    [HttpPost][ValidateAntiForgeryToken] public async Task<IActionResult> Edit(RecipeView model) {
        if (!ModelState.IsValid) {
            await SetAvailableIngredientsAsync();
            return View(model);
        }
        var recipe = await _db.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefaultAsync(r => r.Id == model.Id);
        if (recipe == null)
            return NotFound();

        recipe.Title = model.Title;
        recipe.Description = model.Description;
        recipe.Tags = model.Tags;

        if (model.ImageFile != null && model.ImageFile.Length > 0) {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "recipes");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await model.ImageFile.CopyToAsync(stream);
            }
            recipe.ImagePath = "/images/recipes/" + uniqueFileName;
        }

        _db.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);

        if (model.Ingredients != null) {
            var groupedIngredientsData = model.Ingredients
                .Where(i => i.IngredientId > 0 && i.Quantity > 0)
                .GroupBy(i => i.IngredientId)
                .Select(g => new RecipeIngredientData {
                    RecipeId = recipe.Id,
                    IngredientId = g.Key,
                    Quantity = g.Sum(i => i.Quantity)
                });
            await _db.RecipeIngredients.AddRangeAsync(groupedIngredientsData);
        }
        await _db.SaveChangesAsync();
        float totalCalories = 0f;
        var recipeIngredients = await _db.RecipeIngredients
            .Where(ri => ri.RecipeId == recipe.Id)
            .Include(ri => ri.Ingredient)
            .ToListAsync();
        foreach (var ri in recipeIngredients) {
            totalCalories += (ri.Ingredient?.Calories ?? 0) * ri.Quantity;
        }
        recipe.Calories = totalCalories;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous] public async Task<IActionResult> Details(int? id) {
        if (id == null) return NotFound();
        var recipe = await _db.Recipes
            .Include(r => r.Author)
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (recipe == null) return NotFound();
        var model = new RecipeView {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            Calories = recipe.Calories,
            Tags = recipe.Tags,
            AuthorId = recipe.AuthorId,
            AuthorUsername = recipe.Author?.Username,
            Ingredients = recipe.RecipeIngredients
                .Select(ri => new RecipeIngredientView {
                    IngredientId = ri.IngredientId,
                    IngredientName = ri.Ingredient?.Name,
                    Quantity = ri.Quantity,
                    Unit = ri.Ingredient.Unit
                }).ToList(), };
        return View(model);
    }

    public async Task<IActionResult> Delete(int? id) {
        if (id == null) return NotFound();
        var recipe = await _db.Recipes
            .Include(r => r.Author)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (recipe == null) return NotFound();
        var model = new RecipeView {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            Tags = recipe.Tags,
            Calories = recipe.Calories,
            AuthorId = recipe.AuthorId,
            AuthorUsername = recipe.Author?.Username
        };
        return View(model);
    }

    [HttpPost, ActionName("Delete")] [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) {
        var recipe = await _db.Recipes.Include(r => r.RecipeIngredients).FirstOrDefaultAsync(r => r.Id == id);
        if (recipe != null) {
            _db.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);
            _db.Recipes.Remove(recipe);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
    private async Task SetAvailableIngredientsAsync() {
        ViewBag.AvailableIngredients = await _db.Ingredients
            .Select(i => new IngredientView { Id = i.Id, Name = i.Name, Unit = i.Unit })
            .ToListAsync();
    }
}