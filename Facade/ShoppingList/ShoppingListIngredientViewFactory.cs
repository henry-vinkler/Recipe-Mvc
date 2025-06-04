using RecipeMvc.Data.Entities;

namespace RecipeMvc.Facade.ShoppingList;

public sealed class ShoppingListIngredientViewFactory
    : AbstractViewFactory<ShoppingListIngredientData, ShoppingListIngredientView>
{
    public ShoppingListIngredientView Create(ShoppingListIngredientData d) => new ShoppingListIngredientView
    {
        ShoppingListId = d.ShoppingListId,
        IngredientId = d.IngredientId,
        Quantity = d.Quantity,
        IsChecked = d.IsChecked
    };

    public IList<ShoppingListIngredientView> CreateMany(IEnumerable<ShoppingListIngredientData> items)
        => items.Select(Create).ToList();
}

