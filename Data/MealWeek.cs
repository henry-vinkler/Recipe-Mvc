

namespace RecipeMvc.Data;
// VAHEKLASS MEALI JA WEEKI VAHEL
public sealed class MealWeek : EntityData<MealWeek>
{
    public int MealId { get; set; }
    public MealData? Meal { get; set; }

    public int WeekDataId { get; set; }
    public WeekData? Week { get; set; }
}
