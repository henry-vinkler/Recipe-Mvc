using RecipeMvc.Facade.Recipe;
using RecipeMvc.Facade;
using Microsoft.AspNetCore.Components.Forms;

namespace RecipeMvc.Tests.Facade;

[TestClass] public class RecipeViewTests : SealedTests<RecipeView, EntityView> {
    [TestMethod] public override void DisplayNameTest() => isDisplayName("Recipe");
    [TestMethod] public void IdTest() => isProperty<int>();
    [TestMethod] public void AuthorIdTest() => isProperty<int>();
    [TestMethod] public void AuthorUsernameTest() => isProperty<string>();
    [TestMethod] public void TitleTest() => isProperty<string>();
    [TestMethod] public void DescriptionTest() => isProperty<string>();
    [TestMethod] public void ImageFileTest() => isProperty<IBrowserFile?>();
    [TestMethod] public void ImagePathTest() => isProperty<string?>();
    [TestMethod] public void CaloriesTest() => isProperty<float>();
    [TestMethod] public void TagsTest() => isProperty<string>();
    [TestMethod] public void IngredientsTest() => isProperty<IList<RecipeIngredientView>>();
}