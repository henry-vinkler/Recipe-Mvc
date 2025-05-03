using RecipeMvc.Data;


namespace RecipeMvc.Domain;

public class RecipeIngredient(RecipeIngredientData? d) : Entity<RecipeIngredientData>(d) {
    public int RecipeId { get; set; }
    public int IngredientId { get; set; }
    public float Quantity { get; set; }
}
