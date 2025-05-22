using RecipeMvc.Aids;
using RecipeMvc.Facade;

public sealed class PlannedRecipeView : EntityView
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int RecipeId { get; set; }
    public string RecipeTitle { get; set; }
    public MealType MealType { get; set; }
    public string Day { get; set; }
    public DateTime DateOfMeal { get; set; }
}
