namespace RecipeMvc.Data;

public sealed class RecipeIngredientData : EntityData<RecipeIngredientData> {
    public int RecipeId { get; set; }
    public int IngredientId { get; set; }
    public float Quantity { get; set; }
}