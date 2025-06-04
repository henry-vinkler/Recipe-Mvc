namespace RecipeMvc.Data.Entities;

public sealed class FavouriteData : EntityData<FavouriteData> {
    public int UserId { get; set; }
    public int RecipeId { get; set; }
}