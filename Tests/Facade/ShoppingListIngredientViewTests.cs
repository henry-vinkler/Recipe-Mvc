using RecipeMvc.Facade;

namespace RecipeMvc.Tests.Facade;

[TestClass]
public class ShoppingListIngredientViewTests : SealedTests<ShoppingListIngredientView, EntityView>
{

    [TestMethod] public void ShoppingListIdTest() => isProperty<int>();
    [TestMethod] public void IngredientIdTest() => isProperty<int>();
    [TestMethod] public void IngredientNameTest() => isProperty<string>();
    [TestMethod] public void QuantityTest() => isProperty<string>();
    [TestMethod] public void UnitTest() => isProperty<string>();
    [TestMethod] public void IsCheckedTest() => isProperty<bool>();
}
