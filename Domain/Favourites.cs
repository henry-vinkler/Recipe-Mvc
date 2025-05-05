using RecipeMvc.Data;

namespace RecipeMvc.Domain;

public class Favourites(FavouritesData? d) : Entity<FavouritesData>(d) {
    public int UserId { get; set; }
    public int RecipeId { get; set; }
}
