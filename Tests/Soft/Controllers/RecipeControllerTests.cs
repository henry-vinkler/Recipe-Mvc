using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data.Entities;
using RecipeMvc.Data.DbContext;
using RecipeMvc.Facade.Recipe;
using RecipeMvc.Soft.Controllers;
using Xunit;

namespace RecipeMvc.Tests.Soft.Controllers;

public class RecipesControllerTests {
    private ApplicationDbContext GetDbContext(string dbName) {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        var db = new ApplicationDbContext(options);
        if (!db.Ingredients.Any()){
            db.Ingredients.AddRange(
                new IngredientData { Id = 1, Name = "Tomato", Calories = 20, Unit = "g" },
                new IngredientData { Id = 2, Name = "Pasta", Calories = 100, Unit = "g" }
            );
            db.SaveChanges();
        }
        if (!db.Recipes.Any()) {
            db.Recipes.AddRange(
                new RecipeData { Id = 1, Title = "Pasta", AuthorId = 1, Tags = "Dinner", Calories = 500 },
                new RecipeData { Id = 2, Title = "Salad", AuthorId = 2, Tags = "Lunch", Calories = 200 }
            );
            db.SaveChanges();
        }
        return db;
        if (!db.RecipeIngredients.Any()) {
            db.RecipeIngredients.AddRange(
                new RecipeIngredientData { Id = 1, RecipeId = 1, IngredientId = 2, Quantity = 50 },
                new RecipeIngredientData { Id = 2, RecipeId = 2, IngredientId = 1, Quantity = 30 }
            );
            db.SaveChanges();
        }
        return db;
    }
    private RecipesController GetController(ApplicationDbContext db, int userId = 1){
        var controller = new RecipesController(db);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]{
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "mock"));
        controller.ControllerContext = new ControllerContext {
            HttpContext = new DefaultHttpContext { User = user }
        };
        return controller;
    }
    [Fact] public async Task Details_ReturnsNotFound_WhenIdIsNull() {
        var db = GetDbContext("DetailsNullDb");
        var controller = GetController(db);

        var result = await controller.Details(null);

        Xunit.Assert.IsType<NotFoundResult>(result);
    }
    [Fact] public async Task Details_ReturnsNotFound_WhenRecipeDoesNotExist(){
        var db = GetDbContext("DetailsNotFoundDb");
        var controller = GetController(db);
        var result = await controller.Details(999);
        Xunit.Assert.IsType<NotFoundResult>(result);
    }
    [Fact] public async Task Create_Get_ReturnsViewResult(){
        var db = GetDbContext("CreateGetDb");
        var controller = GetController(db);
        var result = await controller.Create();
        Xunit.Assert.IsType<ViewResult>(result);
    }
    [Fact] public async Task Create_Post_InvalidModel_ReturnsViewWithModel() {
        var db = GetDbContext("CreatePostInvalidDb");
        var controller = GetController(db);

        controller.ModelState.AddModelError("Title", "Required");

        var model = new RecipeView();
        var result = await controller.Create(model);

        var viewResult = Xunit.Assert.IsType<ViewResult>(result);
        Xunit.Assert.Equal(model, viewResult.Model);
    }
    [Fact]public async Task Create_Post_ValidModel_RedirectsToIndex(){
        var db = GetDbContext("CreatePostValidDb");
        var controller = GetController(db);
        var model = new RecipeView{
            Title = "Soup",
            Description = "Hot soup",
            Tags = "Dinner",
            Ingredients = new List<RecipeIngredientView>()
        };
        var result = await controller.Create(model);
        var redirect = Xunit.Assert.IsType<RedirectToActionResult>(result);
        Xunit.Assert.Equal("Index", redirect.ActionName);
    }
    [Fact]public async Task Edit_Get_ReturnsNotFound_WhenIdIsNull(){
        var db = GetDbContext("EditGetNullDb");
        var controller = GetController(db);
        var result = await controller.Edit(null);
        Xunit.Assert.IsType<NotFoundResult>(result);
    }
    [Fact] public async Task Edit_Get_ReturnsNotFound_WhenRecipeDoesNotExist(){
        var db = GetDbContext("EditGetNotFoundDb");
        var controller = GetController(db);
        var result = await controller.Edit(999);
        Xunit.Assert.IsType<NotFoundResult>(result);
    }

    [Fact] public async Task Edit_Get_ReturnsViewResult_WhenRecipeExists() {
        var db = GetDbContext("EditGetFoundDb");
        var controller = GetController(db);
        var result = await controller.Edit(1);
        Xunit.Assert.IsType<ViewResult>(result);
    }
}

