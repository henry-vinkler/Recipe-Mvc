using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Controllers;
using RecipeMvc.Tests.Soft.Controllers;
using RecipeMvc.Aids;
using System.Reflection;

namespace Mvc.Tests.Soft.Controllers;

[TestClass]
public class PlannedRecipeControllerTests : ControllerBaseTests<PlannedRecipeController, PlannedRecipe, PlannedRecipeData, PlannedRecipeView>
{
    protected override PlannedRecipe? createEntity(Func<PlannedRecipeData> getData)
        => new(getData());

    protected override PlannedRecipeController createObj() => new(dbContext!);

    [TestMethod] public void HasDbContextFieldTest() => notNull(typeof(PlannedRecipeController).GetField("_db", BindingFlags.NonPublic | BindingFlags.Instance));
    [TestMethod] public void HasViewFactoryFieldTest() => notNull(typeof(PlannedRecipeController).GetField("_viewFactory", BindingFlags.NonPublic | BindingFlags.Static));
    [TestMethod] public void HasEntityFactoryFieldTest() => notNull(typeof(PlannedRecipeController).GetField("_entityFactory", BindingFlags.NonPublic | BindingFlags.Static));
    [TestMethod] public void HasPageSizeFieldTest() => notNull(typeof(PlannedRecipeController).GetField("pageSize", BindingFlags.NonPublic | BindingFlags.Instance));

    [TestMethod] public void CanCallWeekView() => notNull(obj?.WeekView().Result);
    [TestMethod] public void CanCallDayView() => notNull(obj?.DayView().Result);
    [TestMethod] public void CanCallAddToDay() => notNull(obj?.AddToDay(DateTime.Today, 1, MealType.Lunch, Days.Monday).Result);
    [TestMethod] public void CanCallRemoveFromDay() => notNull(obj?.RemoveFromDay(1, DateTime.Today, Days.Monday).Result);
    [TestMethod] public void CanCallIndex() => notNull(obj?.Index().Result);
}
