using RecipeMvc.Data;

namespace RecipeMvc.Domain;

public class ShoppingListIngredient(ShoppingListIngredientData? d) : Entity<ShoppingListIngredientData>(d)
{    
    public int ShoppingListID => Data.ShoppingListID;
    public int IngredientID => Data.IngredientID;
    public string Quantity => Data.Quantity;
    public bool IsChecked => Data.IsChecked;
}
