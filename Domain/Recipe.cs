using RecipeMvc.Data;
using RecipeMvc.Domain;
namespace RecipeMvc.Domain;

public class Recipe(RecipeData? d) : Entity<RecipeData>(d)
{
    public int AuthorId => Data.AuthorId;
    public string Title => Data.Title;
    public string Description => Data.Description;
    public string? ImagePath => Data.ImagePath;
    public float Calories => Data.Calories;
    public string Tags => Data.Tags;
}
