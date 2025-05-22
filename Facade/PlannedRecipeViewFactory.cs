using RecipeMvc.Data;
using RecipeMvc.Facade;

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
