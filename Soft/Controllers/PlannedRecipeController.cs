using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Aids;
using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Controllers;
using RecipeMvc.Soft.Data;

public class PlannedRecipeController(ApplicationDbContext db)
    : BaseController<PlannedRecipe, PlannedRecipeData, PlannedRecipeView>
        (db, new PlannedRecipeViewFactory(), db => new(db))
{
    [HttpGet]
    public async Task<IActionResult> DayView(DateTime date)
    {
        var Date = await db.MealPlans.FirstOrDefaultAsync();
        if (Date != null) date = Date.DateOfMeal;
        var plan = await db.MealPlans.FirstOrDefaultAsync(p => p.DateOfMeal.Date == date);
        if (plan == null) return NotFound();

        var planned = await db.PlannedRecipes
            .Where(p => p.MealPlanId == plan.Id)
            .ToListAsync();
    
        var viewModels = new List<PlannedRecipeView>();
        foreach (var p in planned)
        {
            var recipe = await db.Recipes.FirstOrDefaultAsync(r => r.Id == p.RecipeId);
            viewModels.Add(new PlannedRecipeView
            {
                Id = p.Id,
                MealPlanId = p.MealPlanId,
                RecipeId = p.RecipeId,
                MealType = p.MealType,
                RecipeTitle = recipe?.Title ?? ""
            });
        }

        // Drop-down jaoks kõik retseptid
        var allRecipes = await db.Recipes.ToListAsync();
        ViewBag.AvailableRecipes = allRecipes.Select(r => new PlannedRecipeView {
            Id = r.Id,
            RecipeTitle = r.Title
        }).ToList();
        ViewBag.Date = date;
        return View(viewModels);
    }

    [HttpPost]
    public async Task<IActionResult> AddToDay(DateTime date, int recipeId, MealType mealType)
    {
        await AddPlannedRecipe(date, recipeId, mealType);
        return RedirectToAction("DayView", new { date });
    }

    private async Task AddPlannedRecipe(DateTime date, int recipeId, MealType mealType)
    {
        var plan = await db.MealPlans.FirstOrDefaultAsync(p => p.DateOfMeal == date);
        if (plan == null)
        {
            plan = new MealPlanData { DateOfMeal = date, UserId = 1 };
            db.MealPlans.Add(plan);
            await db.SaveChangesAsync();
        }

        var planned = new PlannedRecipeData
        {
            MealPlanId = plan.Id,
            RecipeId = recipeId,
            MealType = mealType
        };
        db.PlannedRecipes.Add(planned);
        await db.SaveChangesAsync();
    }
    
    [HttpPost]
    public async Task<IActionResult> RemoveFromDay(int id, DateTime date)
    {
        var item = await db.PlannedRecipes.FindAsync(id);
        if (item != null)
        {
            db.PlannedRecipes.Remove(item);
            await db.SaveChangesAsync();
        }
         return RedirectToAction("DayView", new { date });
    }

}