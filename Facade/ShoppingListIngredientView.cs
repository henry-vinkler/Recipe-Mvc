
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace RecipeMvc.Facade;

[DisplayName("Shopping List Ingredient")]
public sealed class ShoppingListIngredientView:EntityView
{
    [DisplayName("Shopping List")]
    public int ShoppingListId { get; set; }

    [DisplayName("Ingredient")]
    public int IngredientId { get; set; }
    [DisplayName("Name")]
    public string? IngredientName { get; set; } = string.Empty;

    [DisplayName("Quantity")]
    [StringLength(40, MinimumLength = 1)]
    [Required(ErrorMessage = "Quantity is required.")]
    public string Quantity { get; set; } = string.Empty;

    [DisplayName("Unit")]
    public string? Unit { get; set; } = string.Empty;

    [DisplayName("Checked")]
    public bool IsChecked { get; set; } 
}
