using RecipeMvc.Aids;

namespace RecipeMvc.Data
{
    public sealed class WeekData : EntityData<WeekData>
    {
        public Days Day { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; } 
        //public ICollection<MealWeek>? MealWeeks { get; set; } // see on justkui 1 nädal
    }
}