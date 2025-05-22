using RecipeMvc.Aids;

namespace RecipeMvc.Data;

public sealed class PlannedRecipeData : EntityData<PlannedRecipeData>
{
    public int AuthorId { get; set; }
    public UserAccountData? Author { get; set; }
    public int MealPlanId { get; set; }
    public int RecipeId { get; set; }
    public RecipeData? Recipe { get; set; }
    public MealType MealType { get; set; }
    public Days Day { get; set; }
    public DateTime DateOfMeal { get; set; }
}
