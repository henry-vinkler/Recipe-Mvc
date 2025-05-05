using System.ComponentModel;

namespace RecipeMvc.Facade;

[DisplayName("Favourites")]
public sealed class FavouritesView : EntityView {
    public int UserId { get; set; }

    public int RecipeId { get; set; }

    [DisplayName("Title")]
    public string? RecipeTitle { get; set; }

    [DisplayName("Image")]

    public string? RecipeImagePath { get; set; }
}
