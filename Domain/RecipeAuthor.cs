using RecipeMvc.Data;

namespace RecipeMvc.Domain;
public class RecipeAuthor(RecipeAuthorData? d) : Entity<RecipeAuthorData>(d)
{
    public int RecipeId => Data.RecipeId;
    public int UserAccountId => Data.UserAccountId;
    public Recipe? Recipe => recipe;
    public UserAccount? UserAccount => user;
    internal Recipe? recipe;
    internal UserAccount? user;
}
