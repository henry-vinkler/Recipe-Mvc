namespace RecipeMvc.Data.Entities;
public sealed class RecipeData : EntityData<RecipeData>
{
    public int AuthorId { get; set; }
    public UserAccountData? Author { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public float Calories { get; set; }
    public string? Tags { get; set; }
}
