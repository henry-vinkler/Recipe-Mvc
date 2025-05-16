using RecipeMvc.Aids;
using RecipeMvc.Data;

public sealed class MealTabelData : EntityData<MealTabelData>
{
    public MealType MealType { get; set; }
    public int RecipeId { get; set; }
}