using RecipeMvc.Data.Entities;
namespace RecipeMvc.Domain;

public class Favourite(FavouriteData? d) : Entity<FavouriteData>(d) {
    public int? UserId => Data?.UserId;
    public int? RecipeId => Data?.RecipeId;
    public RecipeData? Recipe { get; set; }
}
