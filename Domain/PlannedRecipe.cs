using RecipeMvc.Aids;
using RecipeMvc.Aids.Enums;
using RecipeMvc.Data;
using RecipeMvc.Domain;

public class PlannedRecipe(PlannedRecipeData? d) : Entity<PlannedRecipeData>(d)
{
    public int MealPlanId => Data.MealPlanId;
    public int RecipeId => Data.RecipeId;
    public MealType MealType  {get; set; }
    //public Days days { get; set; }
    public Recipe? Recipe { get; set; }
}

    