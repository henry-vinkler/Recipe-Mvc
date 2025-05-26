namespace RecipeMvc.Tests.Facade;


[TestClass]
public class PlannedRecipeViewTests : SealedTests<PlannedRecipeView, EntityView>
{
    [TestMethod] public override void DisplayNameTest() => isDisplayName("PlannedRecipeView");
    [TestMethod] public void IdTest() => isProperty<int>();
    [TestMethod] public void AuthorIdTest() => isProperty<int>();
    [TestMethod] public void RecipeIdTest() => isProperty<int>();
    [TestMethod] public void RecipeTitleTest() => isProperty<string>();
    [TestMethod] public void MealTypeTest() => isProperty<RecipeMvc.Aids.MealType>();
    [TestMethod] public void DayTest() => isProperty<string>();
    [TestMethod] public void DateOfMealTest() => isProperty<DateTime>();
    [TestMethod] public void CaloriesTest() => isProperty<float>();
}
