using RecipeMvc.Data;
using RecipeMvc.Data.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;

namespace RecipeMvc.Soft.Controllers;

public class ShoppingListIngredientsController(ApplicationDbContext c) 
    :BaseController<ShoppingListIngredient, ShoppingListIngredientData, ShoppingListIngredientView>(c, new ShoppingListIngredientViewFactory(), d => new ShoppingListIngredient(d)) { }