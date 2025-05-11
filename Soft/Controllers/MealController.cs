using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Soft.Controllers;

public class MealController(ApplicationDbContext c)
    : BaseController<Meal, MealPlanData, MealView>(c, new MealViewFactory(), d => new(d))
{
    public async Task<IActionResult> GetRecipes()
    {
        var recipes = await c.Recipes.ToListAsync();
        return View(recipes);
    }

    public async Task<IActionResult> Index()
    {
        var meals = await c.MealWeeks
            .Include(mw => mw.Meal)
                .ThenInclude(m => m.Recipe)
            .Include(mw => mw.Week)
            .ToListAsync();

        return View(meals);
    }
    public async Task<IActionResult> Plan()
    {
        var model = new MealView
        {
            WeekDays = await c.Weeks.OrderBy(d => d.Day).ToListAsync(),
            Recipes = await c.Meal
                .Include(m => m.Recipe)
                .ToListAsync(),
        };
        return View(model);
    }
[HttpPost]
public async Task<IActionResult> SelectDay(int SelectedDayId)
{
    var model = new MealView
    {
        WeekDays = await c.Weeks.OrderBy(d => d.Day).ToListAsync(),
        Recipes = await c.Meal.ToListAsync(),
        SelectedDayId = SelectedDayId
    };
    return View("Plan", model);
}

[HttpPost]
public async Task<IActionResult> AssignRecipeToDay(int SelectedDayId, int SelectedRecipeId)
{
    var assignment = new MealWeek
    {
        WeekDataId = SelectedDayId,
        MealId = SelectedRecipeId // kui see tähistab Recipe.Id
    };
    c.MealWeeks.Add(assignment);
    await c.SaveChangesAsync();
    return RedirectToAction("Plan");
}
}
