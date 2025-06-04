using System.ComponentModel;
namespace RecipeMvc.Facade.Favorite;

[DisplayName("Favourite")]
public sealed class FavouriteView : EntityView {
    public int UserId { get; set; }
    public int RecipeId { get; set; }

    [DisplayName("Title")]
    public string? RecipeTitle { get; set; }

    [DisplayName("Image")]
    public string? RecipeImagePath { get; set; }
}
