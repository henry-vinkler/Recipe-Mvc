using RecipeMvc.Data;
namespace RecipeMvc.Domain;
public class Recipe(RecipeData? d) : Entity<RecipeData>(d)
{
    public int AuthorId => Data.AuthorId;
    internal List<RecipeAuthor> RecipeAuthors = [];
    public List<UserAccount?> Users => RecipeAuthors?
        .Where(r => r.UserAccount is not null).Select(r => r.UserAccount).ToList() ?? [];
    public string Title => Data.Title;
    public string Description => Data.Description;
    public string ImagePath => Data.ImagePath;
    public float Calories => Data.Calories;
    public string Tags => Data.Tags;
}
