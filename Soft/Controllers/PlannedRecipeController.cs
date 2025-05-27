using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Soft.Data;
using RecipeMvc.Aids;
using RecipeMvc.Facade;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace RecipeMvc.Soft.Controllers
{
    [Authorize]
    public class PlannedRecipeController : BaseController<PlannedRecipe, PlannedRecipeData, PlannedRecipeView>
    {
        private readonly ApplicationDbContext _db;
        private const byte pageSize = 6;

        private static readonly PlannedRecipeViewFactory _viewFactory = new();
        private static readonly Func<PlannedRecipeData?, PlannedRecipe> _entityFactory = d => new PlannedRecipe(d);

        public PlannedRecipeController(ApplicationDbContext db)
            : base(db, _viewFactory, _entityFactory)
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

        // Sum calories for planned recipes
        private int GetTotalCalories(List<PlannedRecipeView> plannedViews)
        {
            if (plannedViews == null || plannedViews.Count == 0) return 0;
            return (int)plannedViews.Sum(p => p.Calories);
        }

        // Show all planned recipes for a specific day
        [HttpGet]
        public async Task<IActionResult> DayView(DateTime? date = null, Days? day = null, string? searchString = null)
        {
            var baseDate = date?.Date ?? DateTime.Today;
            DateTime actualDate;
            if (day != null) {
                var weekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek + (int)DayOfWeek.Monday);
                actualDate = weekStart.AddDays((int)day);
            } else {
                actualDate = baseDate;
            }
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out var userId))
                return Challenge();

            var planned = await GetPlannedRecipesForDay(actualDate, day, userId);
            planned = FilterPlannedRecipes(planned, searchString);
            var plannedViews = MapToPlannedRecipeViews(planned);

            ViewBag.Date = actualDate;
            ViewBag.Day = day;
            ViewBag.AllRecipes = await _db.Recipes.Where(r => r.AuthorId == userId).ToListAsync();
            ViewBag.Calories = GetTotalCalories(plannedViews);
            return View(plannedViews);
        }

        private async Task<List<PlannedRecipeData>> GetPlannedRecipesForDay(DateTime actualDate, Days? day, int userId)
        {
            return await _db.PlannedRecipes
                .Include(p => p.Author)
                .Include(p => p.Recipe)
                .Where(p => p.DateOfMeal.Date == actualDate.Date
                    && (day == null || p.Day == day)
                    && p.AuthorId == userId)
                .ToListAsync();
        }

        private List<PlannedRecipeData> FilterPlannedRecipes(List<PlannedRecipeData> planned, string? searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return planned;
            var search = searchString.ToLower();
            DateTime searchDate;
            // Toeta ainult dd.MM.yyyy formaati
            bool isCustomDate = DateTime.TryParseExact(searchString, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out searchDate);
            if (isCustomDate)
            {
                return planned.Where(p => p.DateOfMeal.Date == searchDate.Date).ToList();
            }
            else
            {
                return planned
                    .Where(p =>
                        (p.Recipe != null && p.Recipe.Title != null && p.Recipe.Title.ToLower().Contains(search)) ||
                        p.MealType.ToString().ToLower().Contains(search) ||
                        p.DateOfMeal.ToString("dd.MM.yyyy").Contains(search)
                    ).ToList();
            }
        }

        private List<PlannedRecipeView> MapToPlannedRecipeViews(List<PlannedRecipeData> planned)
        {
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

        [HttpPost]
        public async Task<IActionResult> AddToDay(DateTime date, int recipeId, MealType mealType, Days day, int page = 1)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out var userId))
                return Challenge();

            if (recipeId == 0)
                return RedirectToAction("Index", new { page });

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
            return RedirectToAction("DayView", new { date, day });
            //return RedirectToAction("Index", new { page });
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
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchString, int page = 1)
        {
            // Calculate week start and end for the given page
            var today = DateTime.Today;
            var weekStart = today.AddDays(-((int)today.DayOfWeek - (int)DayOfWeek.Monday) + (page - 1) * 7);
            var weekEnd = weekStart.AddDays(7);

            var recipes = _db.PlannedRecipes
                .Include(r => r.Recipe)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var search = searchString.ToLower();
                recipes = recipes.Where(r =>
                    (r.Recipe != null && r.Recipe.Title != null && r.Recipe.Title.ToLower().Contains(search)) ||
                    r.MealType.ToString().ToLower().Contains(search));
                ViewData["CurrentFilter"] = searchString;
            }

            // Filter by week
            recipes = recipes.Where(r => r.DateOfMeal >= weekStart && r.DateOfMeal < weekEnd);

            // Group by day for the week, but map to PlannedRecipeView
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
            ViewData["TotalPages"] = null; // Optional: calculate if you want to show total weeks
            return View(groupedByDay);
        }
    }
}
