using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeMvc.Facade;

[DisplayName("Recipe Ingredient")]
public class RecipeIngredientView : EntityView {

    public int RecipeId { get; set; }

    [DisplayName("Ingredient")]
    public int IngredientId { get; set; }

    [DisplayName("Name")]
    public string? IngredientName { get; set; }


    [DisplayName("Quantity")]
    [Range(0.01, 10000)]
    public float Quantity { get; set; }
}
