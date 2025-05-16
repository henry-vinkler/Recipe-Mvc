using RecipeMvc.Aids;
using RecipeMvc.Domain;

public class MealTabel(MealTabelData? d) : Entity<MealTabelData>(d)
{
    public MealType MealType { get; set; }
    public int RecipeId { get; set; }
}