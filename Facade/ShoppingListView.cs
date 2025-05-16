using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeMvc.Facade;

[DisplayName("Shopping list")]
public sealed class ShoppingListView : EntityView
{
    [DisplayName("User")]
    public int UserID { get; set; }

    [DisplayName("Name")]
    [StringLength(50, MinimumLength = 1)]
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [DisplayName("Checked")]
    public bool IsChecked { get; set; }

    [DisplayName("Notes")]
    [StringLength(200)]
    public string Notes { get; set; } = string.Empty;
}
