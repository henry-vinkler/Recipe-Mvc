using RecipeMvc.Data.Entities;
namespace RecipeMvc.Domain;

public class RecipeIngredient(RecipeIngredientData? d) : Entity<RecipeIngredientData>(d) {
    public int? RecipeId => Data?.RecipeId;
    public int? IngredientId => Data?.IngredientId;
    public float? Quantity => Data?.Quantity;
    public RecipeData? Recipe { get; set; }
    public IngredientData? Ingredient { get; set; }
}
