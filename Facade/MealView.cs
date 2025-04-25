
    using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeMvc.Facade;

[DisplayName("Meal")]
public class MealView : EntityView
{
    [StringLength(40, MinimumLength = 3)]
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }

 

    [Required(ErrorMessage = "Description must be at least 10 characters long.")]
    [StringLength(200, MinimumLength = 10)]
    public string? Description { get; set; }
}

