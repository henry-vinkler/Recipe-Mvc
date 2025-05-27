using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeMvc.Aids;
using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Controllers;
using RecipeMvc.Soft.Data;
using System;
using System.Collections.Generic;
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

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        return controller;
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

        // Seed üks kasutaja ja retsept
        var user = new UserAccountData
        {
            Id = 1,
            FirstName = "Test1",
            LastName = "User1",
            Email = "test1@example.com",
            Username = "testuser1",
            Password = "password"
        };
        dbContext.UserAccounts.Add(user);
        dbContext.SaveChanges();

        var recipe = new RecipeData
        {
            Id = 1,
            AuthorId = 1,
            Title = "Recipe 1",
            Description = "Description 1",
            ImagePath = null,
            Calories = 100,
            Tags = "tag",
            RecipeIngredients = new List<RecipeIngredientData>()
        };
        dbContext.Recipes.Add(recipe);
        dbContext.SaveChanges();

        var plannedRecipe = new PlannedRecipeData
        {
            Id = 1,
            AuthorId = 1,
            RecipeId = 1,
            MealPlanId = 1,
            MealType = MealType.Lunch,
            Day = Days.Wednesday,
            DateOfMeal = SeededDate
        };
        dbContext.PlannedRecipes.Add(plannedRecipe);
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
    }

    internal protected new void seedData()
    {
        var users = new List<UserAccountData>();
        for (int i = 1; i <= 5; i++)
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

        var recipes = new List<RecipeData>();
        for (int i = 1; i <= 5; i++)
        {
            recipes.Add(new RecipeData
            {
                Id = i,
                AuthorId = users[i - 1].Id,
                Title = $"Recipe {i}",
                Description = $"Description {i}",
                ImagePath = null,
                Calories = 100 * i,
                Tags = "tag",
                RecipeIngredients = new List<RecipeIngredientData>()
            });
        }
        dbContext.Recipes.AddRange(recipes);
        dbContext.SaveChanges();

        var plannedRecipes = new List<PlannedRecipeData>();
        for (int i = 1; i <= 5; i++)
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
        dbContext.PlannedRecipes.AddRange(plannedRecipes);
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

    [TestMethod] public void CanCallIndex() =>
        notNull(obj?.Index(null, 1).Result);
}
