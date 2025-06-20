﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data.DbContext;
using RecipeMvc.Data.Entities;
using RecipeMvc.Domain;
using RecipeMvc.Facade.Recipe;
using System.Security.Claims;
namespace RecipeMvc.Soft.Controllers;

[Authorize] public class RecipesController: BaseController<Recipe, RecipeData, RecipeView> {
    private readonly ApplicationDbContext _db;
    private static readonly RecipeViewFactory _viewFactory = new();
    private const byte pageSize = 6;
    public RecipesController(ApplicationDbContext db): base(db, _viewFactory, d => new Recipe(d)){
        _db = db;
    }
    [AllowAnonymous] public override async Task<IActionResult> Index(int page = 1, string? orderBy = null, string? filter = null) {
        var recipesQuery = _db.Recipes.Include(r => r.Author).AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter)) {
            var search = filter.ToLower();
            recipesQuery = recipesQuery.Where(r =>
                r.Title.ToLower().Contains(search) ||
                r.Tags.ToLower().Contains(search));
            ViewData["CurrentFilter"] = filter;
        }
        if (User.Identity.IsAuthenticated && Request.Query.ContainsKey("mine")) {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            recipesQuery = recipesQuery.Where(r => r.AuthorId == userId);
        }
        var totalCount = await recipesQuery.CountAsync();
        var baseRecipes = await recipesQuery
            .OrderByDescending(r => r.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var recipeIds = baseRecipes.Select(r => r.Id).ToList();
        var ingredientMap = await _db.RecipeIngredients
            .Where(ri => recipeIds.Contains(ri.RecipeId))
            .Join(_db.Ingredients,
                ri => ri.IngredientId,
                ing => ing.Id,
                (ri, ing) => new {
                    ri.RecipeId,
                    Ingredient = new RecipeIngredientView {
                        IngredientId = ing.Id,
                        IngredientName = ing.Name,
                        Quantity = ri.Quantity,
                        Unit = ing.Unit
                    }
                })
            .GroupBy(x => x.RecipeId)
            .ToDictionaryAsync(g => g.Key, g => g.Select(x => x.Ingredient).ToList());
        var recipeViews = new List<RecipeView>();
        foreach (var r in baseRecipes) {
            var ingredients = await GetRecipeIngredientsAsync(r.Id);
            recipeViews.Add(CreateRecipeView(r, ingredients));
        }
        foreach (var recipe in recipeViews) {
            recipe.Calories = CalculateCalories(recipe.Ingredients);
        }
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewData["CurrentFilter"] = filter;
        return View(recipeViews);
    }
    public override async Task<IActionResult> Create() {
        await SetAvailableIngredientsAsync();
        return View(new RecipeView { Ingredients = new List<RecipeIngredientView>() });
    }
    [HttpPost, ValidateAntiForgeryToken] public override async Task<IActionResult> Create(RecipeView model) {
        if (!ModelState.IsValid) {
            await SetAvailableIngredientsAsync();
            return View(model);
        }
        int authorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        string? imagePath = await SaveRecipeImageAsync(model.ImageFile);
        var recipeData = new RecipeData {
            Title = model.Title,
            Description = model.Description,
            Tags = model.Tags,
            AuthorId = authorId,
            ImagePath = imagePath,
            Calories = CalculateCalories(model.Ingredients)
        };
        _db.Recipes.Add(recipeData);
        await _db.SaveChangesAsync();
        await SaveRecipeIngredientsAsync(recipeData.Id, model.Ingredients);
        return RedirectToAction(nameof(Index));
    }
    public override async Task<IActionResult> Details(int? id) {
        if (id == null) return NotFound();
        var recipe = await _db.Recipes
            .Include(r => r.Author)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (recipe == null) return NotFound();
        var ingredients = await GetRecipeIngredientsAsync(recipe.Id);
        var model = CreateRecipeView(recipe, ingredients, recipe.Author?.Username);
        return View(model);
    }
    public override async Task<IActionResult> Edit(int? id) {
        if (id == null) return NotFound();
        var recipe = await _db.Recipes.FirstOrDefaultAsync(r => r.Id == id);
        if (recipe == null) return NotFound();
        var ingredients = await GetRecipeIngredientsAsync(recipe.Id);
        var model = CreateRecipeView(recipe, ingredients);  
        await SetAvailableIngredientsAsync();
        return View(model);
    }
    [HttpPost, ValidateAntiForgeryToken] public override async Task<IActionResult> Edit(int id, RecipeView model) {
        if (id != model.Id) return NotFound();
        if (!ModelState.IsValid) {
            await SetAvailableIngredientsAsync();
            return View(model);
        }
        var recipe = await _db.Recipes.FirstOrDefaultAsync(r => r.Id == model.Id);
        if (recipe == null) return NotFound();
        recipe.Title = model.Title;
        recipe.Description = model.Description;
        recipe.Tags = model.Tags;
        string? imagePath = await SaveRecipeImageAsync(model.ImageFile);
        if (!string.IsNullOrEmpty(imagePath))
            recipe.ImagePath = imagePath;
        var existingIngredients = await _db.RecipeIngredients.Where(ri => ri.RecipeId == recipe.Id).ToListAsync();
        _db.RecipeIngredients.RemoveRange(existingIngredients);
        await SaveRecipeIngredientsAsync(recipe.Id, model.Ingredients);
        recipe.Calories = await _db.RecipeIngredients
            .Where(ri => ri.RecipeId == recipe.Id)
            .Join(_db.Ingredients,
                ri => ri.IngredientId,
                ing => ing.Id,
                (ri, ing) => ing.Calories * ri.Quantity)
            .SumAsync();
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private async Task SetAvailableIngredientsAsync() {
        ViewBag.AvailableIngredients = await _db.Ingredients
            .Select(i => new IngredientView {
                Id = i.Id,
                Name = i.Name,
                Unit = i.Unit
            })
            .ToListAsync();
    }
    private async Task<string?> SaveRecipeImageAsync(IFormFile? imageFile) {
        if (imageFile == null || imageFile.Length == 0)
            return null;
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "recipes");
        Directory.CreateDirectory(uploadsFolder);
        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var stream = new FileStream(filePath, FileMode.Create)) {
            await imageFile.CopyToAsync(stream);
        }
        return "/images/recipes/" + uniqueFileName;
    }
    private async Task SaveRecipeIngredientsAsync(int recipeId, IList<RecipeIngredientView>? ingredients) {
        if (ingredients == null) return;
        var groupedIngredients = ingredients
            .Where(i => i.IngredientId > 0 && i.Quantity > 0)
            .GroupBy(i => i.IngredientId)
            .Select(g => new RecipeIngredientData {
                RecipeId = recipeId,
                IngredientId = g.Key,
                Quantity = g.Sum(i => i.Quantity)
            });
        await _db.RecipeIngredients.AddRangeAsync(groupedIngredients);
        await _db.SaveChangesAsync();
    }
    private float CalculateCalories(IList<RecipeIngredientView>? ingredients) {
        if (ingredients == null) return 0;
        return ingredients.Sum(ing =>
            _db.Ingredients.Where(i => i.Id == ing.IngredientId)
                .Select(i => i.Calories)
                .FirstOrDefault() * ing.Quantity
        );
    }
    private RecipeView CreateRecipeView(RecipeData recipe, IList<RecipeIngredientView> ingredients, string? authorUsername=null) {
        return new RecipeView {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            Tags = recipe.Tags,
            AuthorId = recipe.AuthorId,
            AuthorUsername = authorUsername,
            ImagePath = string.IsNullOrEmpty(recipe.ImagePath) ? null : recipe.ImagePath.StartsWith("/images/recipes/") ? recipe.ImagePath : "/images/recipes/" + recipe.ImagePath,
            Calories = recipe.Calories,
            Ingredients = ingredients
        };
    }
    private async Task<IList<RecipeIngredientView>> GetRecipeIngredientsAsync(int recipeId) {
        return await _db.RecipeIngredients
            .Where(ri => ri.RecipeId == recipeId)
            .Join(_db.Ingredients,
                ri => ri.IngredientId,
                ing => ing.Id,
                (ri, ing) => new RecipeIngredientView {
                    IngredientId = ing.Id,
                    IngredientName = ing.Name,
                    Quantity = ri.Quantity,
                    Unit = ing.Unit
                })
            .ToListAsync();
    }
}