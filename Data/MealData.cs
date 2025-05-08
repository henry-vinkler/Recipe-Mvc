using RecipeMvc.Aids.Enums;

namespace RecipeMvc.Data;

public sealed class MealData : EntityData<MealData>
{
    public int UserId { get; set; }
    public int RecipeId { get; set; }
    //public string? Name { get; set; }
    public string? Note { get; set; }
    public RecipeData? Recipe { get; set; } // see on justkui 1 toit
    
}
