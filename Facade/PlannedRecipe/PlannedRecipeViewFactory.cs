using RecipeMvc.Data.Entities;
using RecipeMvc.Facade;
using RecipeMvc.Facade.PlannedRecipe;

public sealed class PlannedRecipeViewFactory 
    : AbstractViewFactory<PlannedRecipeData, PlannedRecipeView>
{
    public override PlannedRecipeView CreateView(PlannedRecipeData? d)
    {
        var v = base.CreateView(d);
        //v.RecipeTitle = d?.Recipe?.Title;
        return v;
    }
    
}
