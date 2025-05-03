namespace RecipeMvc.Data;
// VAHEKLASS MEALI JA WEEKI VAHEL
public sealed class MealWeek<MealData> : EntityData<MealData> where MealData : EntityData<MealData> // LISADA PARAM WEEK
{
    //public int MealWeekId { get; set; }
    public int MealPlanId { get; set; }
    public int WeekId { get; set; }
}
