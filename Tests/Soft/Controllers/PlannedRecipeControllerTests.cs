using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Controllers;
using RecipeMvc.Tests.Soft.Controllers;

namespace Mvc.Tests.Soft.Controllers;

[TestClass] public class PlannedRecipeControllerTests() :
   ControllerBaseTests<PlannedRecipeController, PlannedRecipe, PlannedRecipeData, PlannedRecipeView> {
    protected override PlannedRecipe? createEntity(Func<PlannedRecipeData> getData)
       => new (getData());

    protected override PlannedRecipeController createObj() => new(dbContext!);
}
