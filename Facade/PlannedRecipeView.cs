using RecipeMvc.Aids;
using RecipeMvc.Facade;

public sealed class PlannedRecipeView : EntityView
{
    public int MealPlanId { get; set; }
    public int RecipeId { get; set; }
    public MealType MealType { get; set; }
    public string? RecipeTitle { get; set; } // kuvamiseks
}
