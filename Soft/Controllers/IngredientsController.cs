using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Soft.Controllers;
public class IngredientsController(ApplicationDbContext c)
    : BaseController<Ingredient, IngredientData, IngredientView>(c, new IngredientViewFactory(), d => new (d)) { }
