using RecipeMvc.Data;

namespace RecipeMvc.Domain
{
    public class Ingredient(IngredientData? d) : Entity<IngredientData>(d)
    {
        public string Name { get; set; }
        public float Calories { get; set; }
        public string Unit { get; set; }
    }
}
