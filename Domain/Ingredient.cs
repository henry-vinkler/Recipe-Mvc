using RecipeMvc.Data;

namespace RecipeMvc.Domain
{
    public class Ingredient(IngredientData? d) : Entity<IngredientData>(d)
    {
        public string Name { get; set; }
        public float Calories { get; set; }
        public string Unit { get; set; }

        /*
        public float proteinPunit { get; set; }
        public float fatPunit { get; set; }
        public float carbPunit { get; set; }
        */
    }
}
