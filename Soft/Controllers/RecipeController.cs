using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Soft.Controllers;
public class RecipeController(ApplicationDbContext c)
    : BaseController<Recipe, RecipeData, RecipeView>(c, new RecipeViewFactory(), d => new(d)) { }
