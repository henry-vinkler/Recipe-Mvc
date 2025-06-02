using RecipeMvc.Data;

namespace RecipeMvc.Tests.Data;

[TestClass] public class ShoppingListDataTests : SealedTests<ShoppingListData, EntityData<ShoppingListData>>
{
    [TestInitialize] public override void Initialize() {
        base.Initialize();
        if (obj == null) return;

        obj.Id = 1;
        obj.UserId = 42;
        obj.Name = "Groceries";
        obj.IsChecked = false;
        obj.Notes = "Buy before weekend";
        obj.Ingredients = new List<ShoppingListIngredientData> {
            new ShoppingListIngredientData { Id = 1, IngredientId = 101, Quantity = "2kg", IsChecked = false },
            new ShoppingListIngredientData { Id = 2, IngredientId = 102, Quantity = "1l", IsChecked = true }
        };
    }

        [TestMethod] public void CloneTest()
        {
            var d = obj?.Clone();
            notNull(d);
            equal(obj.Id, d?.Id);
            equal(obj.UserId, d?.UserId);
            equal(obj.Name, d?.Name);
            equal(obj.IsChecked, d?.IsChecked);
            equal(obj.Notes, d?.Notes);
            notNull(d?.Ingredients);
            equal(obj.Ingredients.Count, d?.Ingredients.Count);
        }

        [TestMethod] public void UserIdTest() => isProperty<int>();
        [TestMethod] public void NameTest() => isProperty<string>();
        [TestMethod] public void IsCheckedTest() => isProperty<bool>();
        [TestMethod] public void NotesTest() => isProperty<string>();
        [TestMethod] public void IngredientsTest() => isProperty<ICollection<ShoppingListIngredientData>>();
    }
}