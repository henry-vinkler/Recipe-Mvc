using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Soft.Controllers;

public class ShoppingListsController(ApplicationDbContext c)
    : BaseController<ShoppingList, ShoppingListData, ShoppingListView>(c, new ShoppingListViewFactory(), d => new(d))
{ }
