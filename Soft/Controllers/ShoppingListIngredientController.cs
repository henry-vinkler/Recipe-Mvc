using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Soft.Controllers;

public class ShoppingListIngredientController(ApplicationDbContext c) 
    :BaseController<ShoppingListIngredient, ShoppingListIngredientData, ShoppingListIngredientView>(c, new ShoppingListIngredientViewFactory(), d => new ShoppingListIngredient(d)) { }