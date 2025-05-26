
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace RecipeMvc.Facade;

[DisplayName("Shopping List Ingredient")]
public sealed class ShoppingListIngredientView:EntityView
{
    [DisplayName("Shopping List")]
    public int ShoppingListID { get; set; }

    [DisplayName("Ingredient")]
    public int IngredientID { get; set; }
    [DisplayName("Name")]
    public string? IngredientName { get; set; }

    [DisplayName("Quantity")]
    [StringLength(40, MinimumLength = 1)]
    [Required(ErrorMessage = "Quantity is required.")]
    public string Quantity { get; set; } = string.Empty;

    [DisplayName("Unit")]
    public string? Unit { get; set; }

    [DisplayName("Checked")]
    public bool IsChecked { get; set; } 
}
