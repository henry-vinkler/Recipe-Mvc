namespace RecipeMvc.Data;

public sealed class FavouritesData : EntityData<FavouritesData> {
    public int UserId { get; set; }
    public int RecipeId { get; set; }
}
