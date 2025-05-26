using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeMvc.Data
{
    public sealed class ShoppingListIngredientData:EntityData<ShoppingListIngredientData>
    {
        public int ShoppingListID { get; set; }
        public int IngredientID { get; set; }
        public string Quantity { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;

        [ForeignKey(nameof(IngredientID))]
        public IngredientData Ingredient { get; set; } = default!;

    }
}
