using RecipeMvc.Aids;

namespace RecipeMvc.Data;

public sealed class PlannedRecipeData : EntityData<PlannedRecipeData>
{
    public int MealPlanId { get; set; }
    public int RecipeId { get; set; }
    public MealType MealType { get; set; }
    
}
