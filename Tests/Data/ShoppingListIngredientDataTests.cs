using RecipeMvc.Data.Entities;

namespace RecipeMvc.Tests.Data;

[TestClass]
public class ShoppingListIngredientDataTests
    : SealedTests<ShoppingListIngredientData, EntityData<ShoppingListIngredientData>>
{

    [TestInitialize] public override void Initialize()
    {
        base.Initialize();
        if (obj == null) return;

        obj.Id = 1;
        obj.ShoppingListId = 10;
        obj.IngredientId = 20;
        obj.Quantity = "500g";
        obj.IsChecked = true;
        obj.Ingredient = new IngredientData
        {
            Id = 20,
            Name = "Flour",
            Unit = "g",
        };
    }

    [TestMethod]
    public void CloneTest()
    {
        var d = obj?.Clone();
        notNull(d);
        equal(obj.Id, d?.Id);
        equal(obj.ShoppingListId, d?.ShoppingListId);
        equal(obj.IngredientId, d?.IngredientId);
        equal(obj.Quantity, d?.Quantity);
        equal(obj.IsChecked, d?.IsChecked);
        notNull(d?.Ingredient);
        equal(obj.Ingredient.Id, d?.Ingredient?.Id);
    }

    [TestMethod] public void ShoppingListIdTest() => isProperty<int>();
    [TestMethod] public void IngredientIdTest() => isProperty<int>();
    [TestMethod] public void QuantityTest() => isProperty<string>();
    [TestMethod] public void IsCheckedTest() => isProperty<bool>();
    [TestMethod] public void IngredientTest() => isProperty<IngredientData>();
}
