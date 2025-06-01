using RecipeMvc.Facade;
using RecipeMvc.Data;
using RecipeMvc.Aids;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecipeMvc.Tests.Facade;

[TestClass]
public class PlannedRecipeViewFactoryTests : SealedTests<PlannedRecipeViewFactory, AbstractViewFactory<PlannedRecipeData, PlannedRecipeView>>
{
    private PlannedRecipeData? data;
    private PlannedRecipeView? view;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        data = crData();
        view = crView();
    }

    [TestCleanup]
    public override void Cleanup()
    {
        base.Cleanup();
        data = null;
        view = null;
    }

    private PlannedRecipeView crView()
    {
        return new PlannedRecipeView
        {
            Id = 1,
            AuthorId = 2,
            RecipeId = 3,
            RecipeTitle = "viewTitle",
            MealType = MealType.Lunch,
            Day = "Wednesday",
            DateOfMeal = new DateTime(2025, 5, 26),
            Calories = 123.4f
        };
    }

    private PlannedRecipeData crData()
    {
        return new PlannedRecipeData
        {
            Id = 1000,
            AuthorId = 2000,
            RecipeId = 3000,
            MealPlanId = 4000,
            MealType = MealType.Dinner,
            Day = RecipeMvc.Aids.Days.Friday,
            DateOfMeal = new DateTime(2025, 5, 30)
        };
    }

    [TestMethod]
    public void CreateViewTest()
    {
        var f = new PlannedRecipeViewFactory();
        var v = f.CreateView(data);
        notNull(v);
        equal(data?.Id, v.Id);
        equal(data?.AuthorId, v.AuthorId);
        equal(data?.RecipeId, v.RecipeId);
        equal(data?.MealType, v.MealType);
        equal(data?.DateOfMeal, v.DateOfMeal);
    }

    [TestMethod]
    public void CreateDataTest()
    {
        var f = new PlannedRecipeViewFactory();
        var d = f.CreateData(view);
        notNull(d);
        equal(view?.Id, d.Id);
        equal(view?.AuthorId, d.AuthorId);
        equal(view?.RecipeId, d.RecipeId);
        equal(view?.MealType, d.MealType);
        equal(view?.DateOfMeal, d.DateOfMeal);
    }
}