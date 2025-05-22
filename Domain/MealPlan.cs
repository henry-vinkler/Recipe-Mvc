using RecipeMvc.Aids;
using RecipeMvc.Data;

namespace RecipeMvc.Domain;

public class MealPlan(MealPlanData? d) : Entity<MealPlanData>(d)
{
    public int UserId => Data?.UserId ?? 0;
    public string? Note => Data?.Note;
    public DateTime DateOfMeal { get; set; }
    public Days WeekDay { get; set; }  
    public virtual ICollection<PlannedRecipe>? PlannedRecipes { get; set; }

}
