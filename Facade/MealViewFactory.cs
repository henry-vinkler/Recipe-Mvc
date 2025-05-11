using RecipeMvc.Data;
using RecipeMvc.Domain;

namespace RecipeMvc.Facade;

public sealed class MealViewFactory : AbstractViewFactory<MealPlanData, MealView>
{
    public override MealView CreateView(MealPlanData? d)
    {
        if (d == null) return new MealView();

        var v = base.CreateView(d);
        v.SelectedRecipeId = d.RecipeId;
        v.Note = d.Note;
        v.Recipes = new List<Meal> {  }; // Või vajadusel tühjenda
        return v;
    }

    public override MealPlanData CreateData(MealView? v)
    {
        if (v == null) return new MealPlanData();

        var d = base.CreateData(v);
        d.RecipeId = v.SelectedRecipeId ?? 0;
        d.Note = v.Note;
        return d;
    }
 }