using RecipeMvc.Data;
namespace RecipeMvc.Domain;
public class Recipe(RecipeData? d) : Entity<RecipeData>(d)
{
    public int AuthorId => Data.AuthorId;
    public string Title => Data.Title;
    public string Description => Data.Description;
    public string ImagePath => Data.ImagePath;
    public float Calories => Data.Calories;
    public string Tags => Data.Tags;
    /*internal List<RecipeAuthor> movieRoles = [];
    public List<UserAccount?> Persons => movieRoles?
        .Where(r => r.Person is not null).Select(r => r.Person).ToList() ?? [];*/
}
