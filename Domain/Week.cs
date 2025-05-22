using RecipeMvc.Aids;
using RecipeMvc.Data;

namespace RecipeMvc.Domain
{
    public class Week(WeekData? d) : Entity<WeekData>(d)
    {
        public Days Day { get; set; } 
        public DateTime ValidFrom { get; set; } 
        public DateTime ValidTo { get; set; } 
    }
}