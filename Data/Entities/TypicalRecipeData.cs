using RecipeMvc.Aids;
using RecipeMvc.Data.Entities;

public class TypicalRecipeData : EntityData<TypicalRecipeData>
{
    public int TypicalDayID { get; set; }
    public int RecipeId { get; set; }
    public MealType meal { get; set; } 
}