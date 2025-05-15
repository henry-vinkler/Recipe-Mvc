using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Soft.Data;
using System;

public class FavouritesController : Controller {

    private readonly ApplicationDbContext _db; 
    public FavouritesController(ApplicationDbContext c) {
        _db = c;
    }

    public async Task<IActionResult> Index() {
        int userId = 1; // asendan päris useriga hiljem

        var favourites = await _db.Favourites
            .Where(f => f.UserId == userId)
            .Include(f => f.Recipe)
            .ToListAsync();

       
        var recipes = favourites
            .Select(f => f.Recipe as Recipe) 
            .Where(r => r != null)
            .ToList();

        return View(recipes);
    }

    [HttpPost] public async Task<IActionResult> Add(int recipeId) {

        int userId = 1;

        bool alreadyExists = await _db.Favourites
            .AnyAsync(f => f.UserId == userId && f.RecipeId == recipeId);

        if (!alreadyExists) {
            var fav = new FavouriteData {
                UserId = userId,
                RecipeId = recipeId
            };

            _db.Favourites.Add(fav);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Recipes");
    }

    [HttpPost] public async Task<IActionResult> Remove(int recipeId)
    {
        int userId = 1;

        var fav = await _db.Favourites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);

        if (fav != null)
        {
            _db.Favourites.Remove(fav);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}

