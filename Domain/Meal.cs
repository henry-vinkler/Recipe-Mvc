using RecipeMvc.Data;

namespace RecipeMvc.Domain;

public class Meal(MealData? d): Entity<MealData>(d)
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
