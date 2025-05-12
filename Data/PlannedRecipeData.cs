

using RecipeMvc.Aids;

namespace RecipeMvc.Data;
// VAHEKLASS MEALI JA WEEKI VAHEL
public sealed class PlannedRecipeData : EntityData<PlannedRecipeData>
{
    public int MealPlanId { get; set; }
    public int RecipeId { get; set; }
    public MealType meal { get; set; } // see on justkui 1 toit
}
