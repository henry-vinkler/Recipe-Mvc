using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeMvc.Aids;
using RecipeMvc.Data.DbContext;
using RecipeMvc.Data.Entities;
using RecipeMvc.Facade.PlannedRecipe;
using RecipeMvc.Soft.Controllers;
using System.Reflection;

namespace RecipeMvc.Tests.Soft.Controllers;

[TestClass]
public class PlannedRecipeControllerTests : ControllerBaseTests<PlannedRecipeController, PlannedRecipe, PlannedRecipeData, PlannedRecipeView>
{
    private static readonly DateTime SeededDate = new(2025, 5, 26);

    protected override PlannedRecipe? createEntity(Func<PlannedRecipeData> getData) => new(getData());

    protected override PlannedRecipeController createObj()
    {
        var controller = new PlannedRecipeController(dbContext!);

        var user = new System.Security.Claims.ClaimsPrincipal(
            new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, "1")
            }, "mock"));

        var httpContext = new DefaultHttpContext { User = user };
        // Inject a fake view engine into the service provider
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        services.AddSingleton<Microsoft.AspNetCore.Mvc.ViewEngines.IViewEngine, FakeViewEngine>();
        httpContext.RequestServices = services.BuildServiceProvider();

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return controller;
    }

    // Add a fake view engine for testing
    private class FakeViewEngine : Microsoft.AspNetCore.Mvc.ViewEngines.IViewEngine
    {
        public Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
            => Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.Found(viewName, new FakeView());
        public Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult GetView(string? executingFilePath, string viewPath, bool isMainPage)
            => Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.Found(viewPath, new FakeView());
        private class FakeView : Microsoft.AspNetCore.Mvc.ViewEngines.IView
        {
            public string Path => "Fake";
            public Task RenderAsync(Microsoft.AspNetCore.Mvc.Rendering.ViewContext context) => Task.CompletedTask;
        }
    }

    private List<UserAccountData> users = new();
    private List<RecipeData> recipes = new();
    private List<PlannedRecipeData> plannedRecipes = new();

    private void SeedUsers(int count = 5)
    {
        users.Clear();
        for (int i = 1; i <= count; i++)
        {
            users.Add(new UserAccountData
            {
                Id = i,
                FirstName = $"Test{i}",
                LastName = $"User{i}",
                Email = $"test{i}@example.com",
                Username = $"testuser{i}",
                Password = "password"
            });
        }
        dbContext!.UserAccounts.AddRange(users);
        dbContext.SaveChanges();
    }

    private void SeedRecipes(int count = 5)
    {
        recipes.Clear();
        for (int i = 1; i <= count; i++)
        {
            recipes.Add(new RecipeData
            {
                Id = i,
                AuthorId = users[i - 1].Id,
                Title = $"Recipe {i}",
                Description = $"Description {i}",
                ImagePath = null,
                Calories = 100 * i,
                Tags = "tag"
            });
        }
        dbContext!.Recipes.AddRange(recipes);
        dbContext!.SaveChanges();
    }

    private void SeedPlannedRecipes(int count = 5)
    {
        plannedRecipes.Clear();
        for (int i = 1; i <= count; i++)
        {
            plannedRecipes.Add(new PlannedRecipeData
            {
                Id = i,
                AuthorId = users[i - 1].Id,
                RecipeId = recipes[i - 1].Id,
                MealPlanId = i,
                MealType = MealType.Lunch,
                Day = Days.Wednesday,
                DateOfMeal = SeededDate
            });
        }
        dbContext!.PlannedRecipes.AddRange(plannedRecipes);
        dbContext!.SaveChanges();
        dbContext.ChangeTracker.Clear();
    }

    [TestInitialize]
    public override void Initialize()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        dbContext = new ApplicationDbContext(options);
        dbContext.Database.OpenConnection();
        dbContext.Database.EnsureCreated();
        dbSet = dbContext.Set<PlannedRecipeData>();

        SeedUsers(1);
        SeedRecipes(1);
        plannedRecipes.Clear();
        plannedRecipes.Add(new PlannedRecipeData
        {
            Id = 1,
            AuthorId = users[0].Id,
            RecipeId = recipes[0].Id,
            MealPlanId = 1,
            MealType = MealType.Lunch,
            Day = Days.Wednesday,
            DateOfMeal = SeededDate
        });
        dbContext.PlannedRecipes.Add(plannedRecipes[0]);
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
    }

    internal protected override void seedData()
    {
        SeedUsers();
        SeedRecipes();
        SeedPlannedRecipes();
        // Paranda kõik PlannedRecipeData AuthorId väärtused vastavaks testikasutajale (ID=1)
        foreach (var pr in dbContext!.PlannedRecipes)
        {
            pr.AuthorId = users[0].Id; // eeldame, et users[0].Id == 1
        }
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
    }

    [TestMethod] public void HasDbContextFieldTest() =>
        notNull(typeof(PlannedRecipeController).GetField("_db", BindingFlags.NonPublic | BindingFlags.Instance));

    [TestMethod] public void HasViewFactoryFieldTest() =>
        notNull(typeof(PlannedRecipeController).GetField("_viewFactory", BindingFlags.NonPublic | BindingFlags.Static));

    [TestMethod] public void HasEntityFactoryFieldTest() =>
        notNull(typeof(PlannedRecipeController).GetField("_entityFactory", BindingFlags.NonPublic | BindingFlags.Static));

    [TestMethod] public void HasPageSizeFieldTest() =>
        notNull(typeof(PlannedRecipeController).GetField("pageSize", BindingFlags.NonPublic | BindingFlags.Static));

    [TestMethod] public void CanCallWeekView() =>
        notNull(obj?.WeekView(SeededDate).Result);

    [TestMethod] public void CanCallDayView() =>
        notNull(obj?.DayView(SeededDate, Days.Wednesday).Result);

    [TestMethod]
    public void CanCallAddToDay()
    {
        var result = obj?.AddToDay(SeededDate, 1, MealType.Lunch, Days.Wednesday).Result;
        notNull(result);
        isType(result, typeof(RedirectToActionResult));
    }

    [TestMethod]
    public void CanCallRemoveFromDay()
    {
        var result = obj?.RemoveFromDay(1, SeededDate, Days.Wednesday).Result;
        notNull(result);
        isType(result, typeof(RedirectToActionResult));
    }

    [TestMethod]
    public void GetWeekStartAndEnd_ReturnsCorrectRange()
    {
        var ctrl = createObj();
        var method = ctrl.GetType().GetMethod("GetWeekStartAndEnd", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(method);
        var tupleObj = method.Invoke(ctrl, new object?[] { new DateTime(2025, 5, 26), 1 });
        Assert.IsNotNull(tupleObj);
        var tuple = ((DateTime, DateTime))tupleObj;
        var (start, end) = tuple;
        Assert.AreEqual(new DateTime(2025, 5, 26).AddDays(-((int)new DateTime(2025, 5, 26).DayOfWeek - (int)DayOfWeek.Monday)), start);
        Assert.AreEqual(start.AddDays(7), end);
    }

    [TestMethod]
    public void MapToPlannedRecipeViews_MapsCorrectly()
    {
        var ctrl = createObj();
        var planned = new List<PlannedRecipeData> {
            new PlannedRecipeData {
                Id = 1,
                RecipeId = 2,
                MealType = MealType.Lunch,
                Day = Days.Wednesday,
                DateOfMeal = new DateTime(2025, 5, 26),
                AuthorId = 1
            }
        };
        var method = ctrl.GetType().GetMethod("MapToPlannedRecipeViews", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(method);
        var resultObj = method.Invoke(ctrl, new object?[] { planned });
        Assert.IsNotNull(resultObj);
        var result = resultObj as List<PlannedRecipeView>;
        notNull(result);
        Assert.AreEqual(1, result!.Count);
        Assert.AreEqual(1, result[0].Id);
        Assert.AreEqual(2, result[0].RecipeId);
    }

    [TestMethod]
    public void GetTotalCalories_ReturnsSum()
    {
        var ctrl = createObj();
        var views = new List<PlannedRecipeView> {
            new PlannedRecipeView { Calories = 100 },
            new PlannedRecipeView { Calories = 200 }
        };
        var method = ctrl.GetType().GetMethod("GetTotalCalories", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(method);
        var sumObj = method.Invoke(ctrl, new object?[] { views });
        Assert.IsNotNull(sumObj);
        var sum = (int)sumObj;
        Assert.AreEqual(300, sum);
    }
}
