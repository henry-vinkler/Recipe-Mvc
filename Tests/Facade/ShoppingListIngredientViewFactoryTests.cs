using RecipeMvc.Data;
using RecipeMvc.Facade;

namespace RecipeMvc.Tests.Facade;

[TestClass] public class ShoppingListIngredientViewFactoryTests
    : SealedTests<ShoppingListViewFactory, AbstractViewFactory<ShoppingListData, ShoppingListView>>
{ }

