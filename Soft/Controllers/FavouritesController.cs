using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Data.Data;
using System.Security.Claims;

public class FavouritesController : Controller {

    private readonly ApplicationDbContext _db; 
    public FavouritesController(ApplicationDbContext c) {
        _db = c;
    }

    public async Task<IActionResult> Index() {

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();

        int userId = int.Parse(userIdStr);

        var recipes = await _db.Favourites
            .Where(f => f.UserId == userId)
            .Join(_db.Recipes,
            fav => fav.RecipeId,
            recipe => recipe.Id,
            (fav, recipe) => recipe)
            .ToListAsync();

        return View(recipes);
    }

    [HttpPost] public async Task<IActionResult> Add(int recipeId) {

        if (!User.Identity?.IsAuthenticated ?? false)
        {
            TempData["FavouriteMessage"] = "You must log in to add recipes to favourites.";
            return RedirectToAction("Index", "Recipes");
        }

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int userId = int.Parse(userIdStr);

        bool alreadyExists = await _db.Favourites
            .AnyAsync(f => f.UserId == userId && f.RecipeId == recipeId);

        if (!alreadyExists) {
            var fav = new FavouriteData {
                UserId = userId,
                RecipeId = recipeId
            };

            _db.Favourites.Add(fav);
            await _db.SaveChangesAsync();

            TempData["showFavouriteToast"] = true;
        }
        return RedirectToAction("Index", "Recipes");
    }

    [HttpPost] public async Task<IActionResult> Remove(int recipeId) {

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();

        int userId = int.Parse(userIdStr);

        var fav = await _db.Favourites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);

        if (fav != null) {
            _db.Favourites.Remove(fav);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}

