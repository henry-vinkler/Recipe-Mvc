using RecipeMvc.Data;

namespace RecipeMvc.Facade;

public sealed class ShoppingListIngredientViewFactory
    : AbstractViewFactory<ShoppingListIngredientData, ShoppingListIngredientView>
{
    public ShoppingListIngredientView Create(ShoppingListIngredientData d) => new ShoppingListIngredientView
    {
        ShoppingListID = d.ShoppingListID,
        IngredientID = d.IngredientID,
        Quantity = d.Quantity,
        IsChecked = d.IsChecked
    };

    public IList<ShoppingListIngredientView> CreateMany(IEnumerable<ShoppingListIngredientData> items)
        => items.Select(Create).ToList();
}

