namespace RecipeMvc.Data
{
    public sealed class IngredientData : EntityData<IngredientData>
    {
        public string Name { get; set; }
        public float Calories { get; set; }
        public string Unit { get; set; }
    }
}
