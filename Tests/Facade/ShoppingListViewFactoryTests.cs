using RecipeMvc.Facade;
using RecipeMvc.Data;

namespace RecipeMvc.Tests.Facade;

[TestClass] public class ShoppingListViewFactoryTests :
    SealedTests<ShoppingListViewFactory, AbstractViewFactory<ShoppingListData, ShoppingListView>>{ }
