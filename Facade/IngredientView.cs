using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeMvc.Facade;

[DisplayName("Ingredient")]
public class IngredientView : EntityView
{
    [StringLength(60, MinimumLength = 3)]
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Range(0, 10000, ErrorMessage = "Calories must be between 0 and 10000.")]
    public float Calories { get; set; }

    [Required(ErrorMessage = "Unit is required.")]
    [RegularExpression("^(g|ml)$", ErrorMessage = "Unit must be either 'g' (gram) or 'ml' (milliliter).")]
    public string Unit { get; set; }
}

