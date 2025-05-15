using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeMvc.Facade.Recipe;
[DisplayName("Recipe")]
public class RecipeView : EntityView {
    [Display(Name = "AuthorId")] public int AuthorId { get; set; }

    [StringLength(60, MinimumLength = 3)] [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    [StringLength(200, MinimumLength = 3)] [Required(ErrorMessage = "Add a description.")]
    public string Description { get; set; }
    public IFormFile ?ImageFile { get; set; }
    public string ?ImagePath { get; set; }

    [Range(0, 10000, ErrorMessage = "Calories must be between 0 and 10000.")]
    public float Calories { get; set; }

    [StringLength(60, MinimumLength = 3)] [Required(ErrorMessage = "Tags are required.")]
    public string Tags { get; set; }

    public IList<RecipeIngredientView> Ingredients { get; set; } = new List<RecipeIngredientView>();

    [DisplayName("Favourited by")]
    public int FavouriteCount { get; set; }

    public bool IsFavourited { get; set; }

    public IList<FavouriteView> FavouritedBy { get; set; } = new List<FavouriteView>();

}
