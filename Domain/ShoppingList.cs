using RecipeMvc.Data;

namespace RecipeMvc.Domain;
public sealed class ShoppingList(ShoppingListData? d) : Entity<ShoppingListData>(d)
{
    public int UserID => Data.UserID;
    public string Name => Data.Name;
    public bool IsChecked => Data.IsChecked;
    public string Notes => Data.Notes;
}
