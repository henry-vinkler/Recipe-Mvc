using RecipeMvc.Facade;
using RecipeMvc.Data.Entities;
using RecipeMvc.Facade.ShoppingList;

namespace RecipeMvc.Tests.Facade;

[TestClass] public class ShoppingListViewFactoryTests :
    SealedTests<ShoppingListViewFactory, AbstractViewFactory<ShoppingListData, ShoppingListView>>{ }
