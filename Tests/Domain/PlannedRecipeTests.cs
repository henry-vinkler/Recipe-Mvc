using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Aids;

namespace RecipeMvc.Tests.Domain;

[TestClass]
public class PlannedRecipeTests : BaseClassTests<PlannedRecipe, Entity<PlannedRecipeData>>
{
    protected override PlannedRecipe createObj()
    {
        var d = new PlannedRecipeData
        {
            Id = 1,
            AuthorId = 2,
            RecipeId = 3,
            MealPlanId = 4,
            MealType = MealType.Lunch,
            Day = Days.Wednesday,
            DateOfMeal = new DateTime(2025, 5, 26)
        };
        return new PlannedRecipe(d);
    }

    [TestMethod] public void IdTest() => equal(1, obj?.Id);
    [TestMethod] public void AuthorIdTest() => equal(2, obj?.AuthorId);
    [TestMethod] public void RecipeIdTest() => equal(3, obj?.RecipeId);
    [TestMethod] public void MealPlanIdTest() => equal(4, obj?.MealPlanId);
    [TestMethod] public void MealTypeTest() => equal(MealType.Lunch, obj?.MealType);
    [TestMethod] public void DateOfMealTest() => equal(new DateTime(2025, 5, 26), obj?.DateOfMeal);
    [TestMethod] public void DataTest() => notNull(obj?.Data);
    [TestMethod] public void MealPlanIdTypeTest() => isType(obj?.MealPlanId, typeof(int));
    [TestMethod] public void RecipeIdTypeTest() => isType(obj?.RecipeId, typeof(int));
    [TestMethod] public void AuthorIdTypeTest() => isType(obj?.AuthorId, typeof(int));
    [TestMethod] public void MealTypeTypeTest() => isType(obj?.MealType, typeof(MealType));
    [TestMethod] public void DateOfMealTypeTest() => isType(obj?.DateOfMeal, typeof(DateTime));
    [TestMethod] public void RecipeTypeTest() => isType(obj?.Recipe, typeof(RecipeData));
}
