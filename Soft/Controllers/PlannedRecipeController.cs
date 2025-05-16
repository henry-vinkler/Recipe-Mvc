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
        date = db.MealPlans.ElementAt(0).DateOfMeal;
        // var aa = new MealPlanData { Id = 0, DateOfMeal = date, UserId = 1, Note = "Test" };
        // var aaa = new MealPlanData { Id = 1, DateOfMeal = date, UserId = 1, Note = "TestUks" };
        // var list = new List<MealPlanData> { aa, aaa };
        var plan = await db.MealPlans.FirstOrDefaultAsync(p => p.DateOfMeal == date);
        //var plan = list.FirstOrDefault(p => p.DateOfMeal == date);
        if (plan == null) return NotFound();

        var planned = await db.PlannedRecipes
            .Where(p => p.MealPlanId == plan.Id)
            .ToListAsync();

        var viewModels = planned.Select(p => new PlannedRecipeView
        {
            Id = p.Id,
            MealPlanId = p.MealPlanId,
            RecipeId = p.RecipeId,
        });

        var list = new List<PlannedRecipeView>();
        foreach (var v in viewModels) {
            var recipe = await db.Recipes.FirstOrDefaultAsync(r => r.Id == v.RecipeId);
            if (recipe == null) continue;
            v.RecipeTitle = recipe.Title;
            list.Add(v); ;
        }
        ViewBag.AvailableRecipes = list;
        ViewBag.Date = date;
        return View(viewModels);
    }

    [HttpPost]
    public async Task<IActionResult> AddToDay(DateTime date, int recipeId, MealType mealType)
    {
        var plan = await db.MealPlans.FirstOrDefaultAsync(p => p.DateOfMeal == date);
        if (plan == null)
        {
            plan = new MealPlanData { DateOfMeal = date, UserId = 1 }; // <--- vali loogika
            db.MealPlans.Add(plan);
            await db.SaveChangesAsync();
        }

        var planned = new PlannedRecipeData
        {
            MealPlanId = plan.Id,
            RecipeId = recipeId,
            //MealType = mealType
        };
        db.PlannedRecipes.Add(planned);
        await db.SaveChangesAsync();

        return RedirectToAction("DayView", new { date });
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