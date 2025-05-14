using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Aids;
using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Controllers;
using RecipeMvc.Soft.Data;

public class PlannedRecipeController(ApplicationDbContext db)
    : BaseController<MealPlan, MealPlanData, MealPlanView>(db, new MealPlanViewFactory(), db => new(db))
{
  public async Task<IActionResult> DayView(DateTime date)
    {
        var plan = await db.MealPlans.FirstOrDefaultAsync(p => p.DateOfMeal == date);
        if (plan == null) return NotFound();

        var planned = await db.PlannedRecipes
            .Where(p => p.MealPlanId == plan.Id)
            .Include(p => p.Recipe)
            .ToListAsync();

        var viewModels = planned.Select(p => new PlannedRecipeView
        {
            Id = p.Id,
            MealPlanId = p.MealPlanId,
            RecipeId = p.RecipeId,
            MealType = p.MealType,
            RecipeTitle = p.Recipe?.Title ?? "Not named"
        });

        ViewBag.AvailableRecipes = await db.Recipes.ToListAsync();
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
            MealType = mealType
        };
        db.PlannedRecipes.Add(planned);
        await db.SaveChangesAsync();

        return RedirectToAction("DayView", new { date });
    }
}