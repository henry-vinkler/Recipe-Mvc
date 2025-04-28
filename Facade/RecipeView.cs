using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeMvc.Facade;
[DisplayName("Recipe")]
public class RecipeView : EntityView
{
    public int AuthorId { get; set; }
    [StringLength(60, MinimumLength = 3)] [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }
    [StringLength(200, MinimumLength = 3)] [Required(ErrorMessage = "Add a description.")]
    public string Description { get; set; }
    public string ImagePath { get; set; }
    [Range(0, 10000, ErrorMessage = "Calories must be between 0 and 10000.")]
    public float Calories { get; set; }
    [StringLength(60, MinimumLength = 3)] [Required(ErrorMessage = "Tags are required.")]
    public string Tags { get; set; }
}
