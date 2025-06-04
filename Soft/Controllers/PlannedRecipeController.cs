using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Soft.Data;
using RecipeMvc.Aids;
using RecipeMvc.Facade;
using Microsoft.AspNetCore.Authorization;

namespace RecipeMvc.Soft.Controllers
{
    [Authorize]
    public class PlannedRecipeController : BaseController<PlannedRecipe, PlannedRecipeData, PlannedRecipeView>
    {
        private readonly ApplicationDbContext _db;
        private static readonly PlannedRecipeViewFactory _viewFactory = new();
        private static readonly Func<PlannedRecipeData?, PlannedRecipe> _entityFactory = d => new PlannedRecipe(d);

        public PlannedRecipeController(ApplicationDbContext db)
            : base(db, _viewFactory, _entityFactory){
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> WeekView(DateTime? weekStart = null, int page = 1){
            var (start, end) = GetWeekStartAndEnd(weekStart, page);
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

        [HttpGet]
        public async Task<IActionResult> DayView(DateTime? date = null, Days? day = null){
            var (weekStart, _) = GetWeekStartAndEnd(date);
            DateTime actualDate;
            if (day != null) {
                actualDate = weekStart.AddDays((int)day);
            } else {
                actualDate = date?.Date ?? DateTime.Today;
            }
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out var userId))
                return Challenge();
            var planned = await GetPlannedRecipesForDay(actualDate, day, userId);
            var plannedViews = MapToPlannedRecipeViews(planned);
            ViewBag.Date = actualDate;
            ViewBag.Day = day;
            ViewBag.AllRecipes = await _db.Recipes.Where(r => r.AuthorId == userId).ToListAsync();
            ViewBag.Calories = GetTotalCalories(plannedViews);
            return View(plannedViews);
        }

        [HttpPost]
        public async Task<IActionResult> AddToDay(DateTime date, int recipeId, MealType mealType, Days day, int page = 1){
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out var userId))
                return Challenge();

            if (recipeId == 0)
                return RedirectToAction("Index", new { page });

            var planned = new PlannedRecipeData{
                AuthorId = userId,
                RecipeId = recipeId,
                MealType = mealType,
                Day = day,
                DateOfMeal = date
            };

            _db.PlannedRecipes.Add(planned);
            await _db.SaveChangesAsync();
            return RedirectToAction("DayView", new { date, day });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromDay(int id, DateTime date, Days day){
            var item = await _db.PlannedRecipes.FindAsync(id);
            if (item != null)
            {
                _db.PlannedRecipes.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("DayView", new { date = date.ToString("yyyy-MM-dd"), day });;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1){
            var (weekStart, weekEnd) = GetWeekStartAndEnd(null, page);
            var recipes = _db.PlannedRecipes
                .Include(r => r.Recipe)
                .Where(r => r.DateOfMeal >= weekStart && r.DateOfMeal < weekEnd)
                .AsQueryable();
            var grouped = await recipes.ToListAsync();
            var groupedByDay = grouped
                .GroupBy(r => r.Day)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(p => new PlannedRecipeView {
                        Id = p.Id,
                        RecipeId = p.RecipeId,
                        RecipeTitle = p.Recipe?.Title ?? string.Empty,
                        MealType = p.MealType,
                        Day = p.Day.ToString(),
                        DateOfMeal = p.DateOfMeal
                    }).ToList()
                );
            ViewData["CurrentPage"] = page;
            ViewData["WeekStart"] = weekStart;
            ViewData["WeekEnd"] = weekEnd.AddDays(-1);
            ViewData["TotalPages"] = null; 
            return View(groupedByDay);
        }

        // --- abimeetodid all ---
        private (DateTime weekStart, DateTime weekEnd) GetWeekStartAndEnd(DateTime? date = null, int page = 1)
        {
            var baseDate = date?.Date ?? DateTime.Today;
            var weekStart = baseDate.AddDays(-((int)baseDate.DayOfWeek - (int)DayOfWeek.Monday) + (page - 1) * 7);
            var weekEnd = weekStart.AddDays(7);
            return (weekStart, weekEnd);
        }

        private async Task<List<PlannedRecipeData>> GetPlannedRecipesForDay(DateTime actualDate, Days? day, int userId){
            return await _db.PlannedRecipes
                .Include(p => p.Author)
                .Include(p => p.Recipe)
                .Where(p => p.DateOfMeal.Date == actualDate.Date
                    && (day == null || p.Day == day)
                    && p.AuthorId == userId)
                .ToListAsync();
        }

        private List<PlannedRecipeView> MapToPlannedRecipeViews(List<PlannedRecipeData> planned){
            return planned.Select(p => new PlannedRecipeView
            {
                Id = p.Id,
                RecipeId = p.RecipeId,
                RecipeTitle = p.Recipe?.Title ?? "",
                MealType = p.MealType,
                Day = p.Day.ToString(),
                DateOfMeal = p.DateOfMeal,
                Calories = p.Recipe?.Calories ?? 0
            }).ToList();
        }

        private int GetTotalCalories(List<PlannedRecipeView> plannedViews){
            if (plannedViews == null || plannedViews.Count == 0) return 0;
            return (int)plannedViews.Sum(p => p.Calories);
        }
    }
}
