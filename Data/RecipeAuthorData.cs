namespace RecipeMvc.Data;

public sealed class RecipeAuthorData : EntityData<RecipeAuthorData>
{
    public int RecipeId { get; set; }
    public int UserAccountId { get; set; }
}
