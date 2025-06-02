using RecipeMvc.Data;
using RecipeMvc.Domain;

namespace RecipeMvc.Tests.Domain;

[TestClass] public class ShoppingListTests : BaseClassTests<ShoppingList, Entity<ShoppingListData>>
{

    protected override ShoppingList createObj()
    {
        var d = new ShoppingListData
        {
            Id = 1,
            UserId = 123,
            Name = "list test",
            IsChecked = true,
            Notes = "remember to buy milk"
        };
        return new ShoppingList(d);
    }

    [TestMethod] public void IdTest() => equal(1, obj?.Id);
    [TestMethod] public void UserIDTest() => equal(123, obj?.UserId);
    [TestMethod] public void NameTest() => equal("list test", obj?.Name);
    [TestMethod] public void IsCheckedTest() => equal(true, obj?.IsChecked);
    [TestMethod] public void NotesTest() => equal("remember to buy milk", obj?.Notes);
    [TestMethod] public void DataTest() => notNull(obj?.Data);
    [TestMethod] public void IdTypeTest() => isType(obj?.Id, typeof(int));
    [TestMethod] public void UserIdTypeTest() => isType(obj?.UserId, typeof(int));
    [TestMethod] public void NameTypeTest() => isType(obj?.Name, typeof(string));
    [TestMethod] public void NotesTypeTest() => isType(obj?.Notes, typeof(string));
}
