using RecipeMvc.Data.Entities;

public class TypicalDayData : EntityData<TypicalDayData>
{
    public int UserId { get; set; }
    public string name { get; set; } = string.Empty;
}