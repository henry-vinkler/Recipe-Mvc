using RecipeMvc.Data;

namespace RecipeMvc.Facade;

public sealed class ShoppingListViewFactory
    : AbstractViewFactory<ShoppingListData, ShoppingListView>
{
    public ShoppingListView Create(ShoppingListData d) => new ShoppingListView
    {
        Id = d.Id,
        UserId = d.UserId,
        Name = d.Name,
        IsChecked = d.IsChecked,
        Notes = d.Notes
    };

    public IList<ShoppingListView> CreateMany(IEnumerable<ShoppingListData> items)
        => items.Select(Create).ToList();
}
