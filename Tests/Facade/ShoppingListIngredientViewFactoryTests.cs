using RecipeMvc.Data.Entities;
using RecipeMvc.Facade;
using RecipeMvc.Facade.ShoppingList;

namespace RecipeMvc.Tests.Facade;

[TestClass] public class ShoppingListIngredientViewFactoryTests
    : SealedTests<ShoppingListViewFactory, AbstractViewFactory<ShoppingListData, ShoppingListView>>
{ }

