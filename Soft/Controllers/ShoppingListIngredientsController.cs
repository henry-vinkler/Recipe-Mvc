using RecipeMvc.Data.DbContext;
using RecipeMvc.Data.Entities;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Facade.ShoppingList;

namespace RecipeMvc.Soft.Controllers;

public class ShoppingListIngredientsController(ApplicationDbContext c) 
    :BaseController<ShoppingListIngredient, ShoppingListIngredientData, ShoppingListIngredientView>(c, new ShoppingListIngredientViewFactory(), d => new ShoppingListIngredient(d)) { }