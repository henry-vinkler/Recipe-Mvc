namespace RecipeMvc.Data;

public sealed class MealPlanData : EntityData<MealPlanData>
{
    public int UserId { get; set; }
    public DateTime DateOfMeal { get; set; }
    public string? Note { get; set; }
        
}
