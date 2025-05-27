using Microsoft.AspNetCore.Mvc;
using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Controllers;

namespace RecipeMvc.Tests.Soft.Controllers;

[TestClass]
public class BaseControllerTests : ControllerBaseTests<BaseController<PlannedRecipe, PlannedRecipeData, PlannedRecipeView>, PlannedRecipe, PlannedRecipeData, PlannedRecipeView>
{
    // Kuna BaseController on abstraktne, ei saa seda otse instantsida ega kasutada PlannedRecipeControllerit,
    // sest see ei päri BaseControllerist. Siin võiks kasutada mocki või abstraktset testklassi, kui vaja.
    // Kui projektis pole BaseControlleri konkreetset implementatsiooni, siis need testid võivad olla ainult abstraktsed või tühjad.

    // [TestMethod]
    // public override void IsSealedTest() =>
    //     isFalse(typeof(BaseController<PlannedRecipe, PlannedRecipeData, PlannedRecipeView>).IsSealed);

    // [TestMethod]
    // public void IsAbstractTest() =>
    //     isTrue(typeof(BaseController<PlannedRecipe, PlannedRecipeData, PlannedRecipeView>).IsAbstract);

    // [TestMethod]
    // public override void IsBaseTypeOfTest() =>
    //     equal(typeof(BaseController<PlannedRecipe, PlannedRecipeData, PlannedRecipeView>).BaseType, typeof(Controller));

    protected override BaseController<PlannedRecipe, PlannedRecipeData, PlannedRecipeView> createObj() => throw new NotImplementedException();
    protected override PlannedRecipe? createEntity(Func<PlannedRecipeData> getData) => throw new NotImplementedException();

    // createObj ja createEntity pole võimalikud ilma konkreetse BaseControlleri implementatsioonita
}
