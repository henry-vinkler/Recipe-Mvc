using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeMvc.Data
{
    public sealed class ShoppingListIngredientData:EntityData<ShoppingListIngredientData>
    {
        public int ShoppingListId { get; set; }
        public int IngredientId { get; set; }
        public string Quantity { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;

        [ForeignKey(nameof(IngredientId))]
        public IngredientData Ingredient { get; set; } = default!;

    }
}
