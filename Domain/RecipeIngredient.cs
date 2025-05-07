using RecipeMvc.Data;
namespace RecipeMvc.Domain;

public class RecipeIngredient(RecipeIngredientData? d) : Entity<RecipeIngredientData>(d) {
    public int? RecipeId => Data?.RecipeId;
    public int? IngredientId => Data?.IngredientId;
    public float? Quantity => Data?.Quantity;
}
