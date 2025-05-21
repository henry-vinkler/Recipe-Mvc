namespace RecipeMvc.Data;

public sealed class RecipeIngredientData : EntityData<RecipeIngredientData> {
    public int RecipeId { get; set; }
    public RecipeData Recipe { get; set; }
    public int IngredientId { get; set; }
    public IngredientData Ingredient { get; set; }
    public float Quantity { get; set; }
}