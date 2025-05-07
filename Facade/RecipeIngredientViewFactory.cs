using RecipeMvc.Data;
namespace RecipeMvc.Facade;

public sealed class RecipeIngredientViewFactory : AbstractViewFactory<RecipeIngredientData, RecipeIngredientView> 
{
    protected RecipeIngredientView Create(RecipeIngredientData d)
    {
        return new RecipeIngredientView {
            RecipeId = d.RecipeId,
            IngredientId = d.IngredientId,
            Quantity = d.Quantity,
        };
    }


}
