using RecipeMvc.Data;

namespace RecipeMvc.Domain;
public sealed class ShoppingList(ShoppingListData? d) : Entity<ShoppingListData>(d)
{
    public int UserId=> Data.UserId;
    public string Name => Data.Name;
    public bool IsChecked => Data.IsChecked;
    public string Notes => Data.Notes;
    public ICollection<ShoppingListIngredient> Ingredients { get; set; } = new List<ShoppingListIngredient>();

}
