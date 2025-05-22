using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Soft.Data;
using System.Threading.Tasks;
using System.Linq;
using RecipeMvc.Aids;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace RecipeMvc.Soft.Controllers
{
    [Authorize]
    public class PlannedRecipeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PlannedRecipeController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Show all planned recipes for a week (grouped by day)
        [HttpGet]
        public async Task<IActionResult> WeekView(DateTime? weekStart = null)
        {
            var start = weekStart?.Date ?? DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var end = start.AddDays(7);

            var planned = await _db.PlannedRecipes
                .Include(p => p.Author)
                .Include(p => p.Recipe)
                .Where(p => p.DateOfMeal >= start && p.DateOfMeal < end)
                .ToListAsync();

            var grouped = planned
                .GroupBy(p => p.Day)
                .ToDictionary(g => g.Key, g => g.ToList());

            ViewBag.WeekStart = start;
            ViewBag.AllRecipes = await _db.Recipes.ToListAsync();

            return View(grouped);
        }

        // Show all planned recipes for a specific day
        [HttpGet]
        public async Task<IActionResult> DayView(DateTime date, Days? day = null)
        {
            // Get logged-in user ID
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out var userId))
                return Challenge();

            // Only show planned recipes for the logged-in user
            var planned = await _db.PlannedRecipes
                .Include(p => p.Author)
                .Include(p => p.Recipe)
                .Where(p => p.DateOfMeal.Date == date.Date
                    && (day == null || p.Day == day)
                    && p.AuthorId == userId) // <-- Filter by user
                .ToListAsync();

            var plannedViews = planned.Select(p => new PlannedRecipeView
            {
                Id = p.Id,
                AuthorId = p.AuthorId,
                AuthorName = p.Author != null ? $"{p.Author.FirstName} {p.Author.LastName}" : "",
                RecipeId = p.RecipeId,
                RecipeTitle = p.Recipe?.Title ?? "",
                MealType = p.MealType,
                Day = p.Day.ToString(),
                DateOfMeal = p.DateOfMeal
            }).ToList();

            ViewBag.Date = date;
            ViewBag.Day = day;
            ViewBag.AllRecipes = await _db.Recipes.Where(r => r.AuthorId == userId).ToListAsync();

            return View(plannedViews);
        }

        // Add a planned recipe
        [HttpPost]
        public async Task<IActionResult> AddToDay(DateTime date, int recipeId, MealType mealType, Days day)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out var userId))
                return Challenge();

            if (recipeId == 0)
                return RedirectToAction("DayView");

            var planned = new PlannedRecipeData
            {
                AuthorId = userId,
                RecipeId = recipeId,
                MealType = mealType,
                Day = day,
                DateOfMeal = date
            };

            _db.PlannedRecipes.Add(planned);
            await _db.SaveChangesAsync();

            return RedirectToAction("DayView");
        }

        // Remove a planned recipe
        [HttpPost]
        public async Task<IActionResult> RemoveFromDay(int id, DateTime date, Days day)
        {
            var item = await _db.PlannedRecipes.FindAsync(id);
            if (item != null)
            {
                _db.PlannedRecipes.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("DayView");
        }
    }
}
