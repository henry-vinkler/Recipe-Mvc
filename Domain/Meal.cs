using RecipeMvc.Data;

namespace RecipeMvc.Domain;

public class Meal(MealPlanData? d) : Entity<MealPlanData>(d)
{
    public int UserId { get; set; }
    //public string? Name { get; set; }
    public string? Note => Data?.Note;
    public Recipe? Recipe { get; set; } // see on justkui 1 toit
}
