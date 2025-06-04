using RecipeMvc.Data.Entities;
using RecipeMvc.Domain;

namespace RecipeMvc.Tests.Domain;

[TestClass] public class ShoppingListIngredientTests
    : BaseClassTests<ShoppingListIngredient, Entity<ShoppingListIngredientData>>
{

    protected override ShoppingListIngredient createObj()
    {
        var d = new ShoppingListIngredientData
        {
            Id = 1,
            ShoppingListId = 2,
            IngredientId = 3,
            Quantity = "300ml",
            IsChecked = true
        };
        return new ShoppingListIngredient(d);
    }

    [TestMethod] public void IdTest() => equal(1, obj?.Id);
    [TestMethod] public void ShoppingListIdTest() => equal(2, obj?.ShoppingListId);
    [TestMethod] public void IngredientIdTest() => equal(3, obj?.IngredientID);
    [TestMethod] public void QuantityTest() => equal("300ml", obj?.Quantity);
    [TestMethod] public void IsCheckedTest() => isTrue(obj?.IsChecked ?? false);

    [TestMethod] public void DataTest() => notNull(obj?.Data);

    [TestMethod] public void IdTypeTest() => isType(obj?.Id, typeof(int));
    [TestMethod] public void ShoppingListIdTypeTest() => isType(obj?.ShoppingListId, typeof(int));
    [TestMethod] public void IngredientIdTypeTest() => isType(obj?.IngredientID, typeof(int));
    [TestMethod] public void QuantityTypeTest() => isType(obj?.Quantity, typeof(string));
    [TestMethod] public void IsCheckedTypeTest() => isType(obj?.IsChecked, typeof(bool));
}
