using RecipeMvc.Data;

namespace RecipeMvc.Domain;

public class MealPlan(MealPlanData? d) : Entity<MealPlanData>(d)
{
    public int UserId => Data?.UserId ?? 0;
    public DateTime DateOfMeal => Data?.DateOfMeal ?? DateTime.MinValue;
    public string? Note => Data?.Note;
    public PlannedRecipeData? PlannedMeal { get; set; }

}
