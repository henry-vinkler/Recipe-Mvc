using RecipeMvc.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeMvc.Domain;

public class ShoppingListIngredient(ShoppingListIngredientData? d) : Entity<ShoppingListIngredientData>(d) {
    public int? ShoppingListID => Data?.ShoppingListID;
    public int? IngredientID => Data?.IngredientID;
    public string Quantity => Data?.Quantity ?? string.Empty;
    public bool IsChecked => Data?.IsChecked ?? false;

    public Ingredient? Ingredient { get; set; }
}

