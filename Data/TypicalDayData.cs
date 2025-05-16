using RecipeMvc.Data;

public class TypicalDayData : EntityData<TypicalDayData>
{
    public int UserId { get; set; }
    public string name { get; set; } = string.Empty;
}