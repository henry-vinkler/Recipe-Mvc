using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Soft.Controllers;

public class RecipeAuthorsController(ApplicationDbContext c)
    : BaseController<RecipeAuthor, RecipeAuthorData, RecipeAuthorView>(c, new RecipeAuthorViewFactory(), d => new(d)){ }
