using RecipeMvc.Aids;
using RecipeMvc.Data;
using RecipeMvc.Domain;

public class PlannedRecipe(PlannedRecipeData? d) : Entity<PlannedRecipeData>(d)
{
    public int MealPlanId => Data?.MealPlanId ?? default;
    public int RecipeId => Data?.RecipeId ?? default;
    public int AuthorId => Data?.AuthorId ?? default;
    public MealType MealType => Data?.MealType ?? default;
    public DateTime DateOfMeal => Data?.DateOfMeal ?? default;
    public Recipe? Recipe { get; set; }
}