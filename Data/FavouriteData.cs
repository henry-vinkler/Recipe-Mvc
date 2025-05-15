namespace RecipeMvc.Data;

public sealed class FavouriteData : EntityData<FavouriteData> {
    public int UserId { get; set; }
    public int RecipeId { get; set; }
    public object Recipe { get; set; }
}
