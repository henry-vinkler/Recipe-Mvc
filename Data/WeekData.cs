namespace RecipeMvc.Data
{
    public sealed class WeekData : EntityData<WeekData>
    {
        // public Days Day { get; set; } // Kas on Vaja?
        public DateTime ValidFrom { get; set; } 
        public DateTime ValidTo { get; set; } 
    }
}