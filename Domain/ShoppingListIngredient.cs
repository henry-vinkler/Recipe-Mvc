using RecipeMvc.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeMvc.Domain;

public class ShoppingListIngredient(ShoppingListIngredientData? d) : Entity<ShoppingListIngredientData>(d) {
    public int? ShoppingListId => Data?.ShoppingListId;
    public int? IngredientID => Data?.IngredientId;
    public string Quantity => Data?.Quantity ?? string.Empty;
    public bool IsChecked => Data?.IsChecked ?? false;

    public ICollection<ShoppingListIngredient> Ingredients { get; set; } = new List<ShoppingListIngredient>();

}

