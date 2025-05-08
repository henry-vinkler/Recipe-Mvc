using RecipeMvc.Data;
using RecipeMvc.Domain;

namespace RecipeMvc.Facade;

public sealed class MealViewFactory : AbstractViewFactory<MealData, MealView>
{
    public override MealView CreateView(MealData? d)
    {
        if (d == null) return new MealView();

        var v = base.CreateView(d);
        v.SelectedRecipeId = d.RecipeId;
        v.Note = d.Note;
        v.Recipes = new List<Meal> {  }; // Või vajadusel tühjenda
        return v;
    }

    public override MealData CreateData(MealView? v)
    {
        if (v == null) return new MealData();

        var d = base.CreateData(v);
        d.RecipeId = v.SelectedRecipeId ?? 0;
        d.Note = v.Note;
        return d;
    }
 }