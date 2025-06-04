using RecipeMvc.Aids;

namespace RecipeMvc.Facade.PlannedRecipe {
    public sealed class PlannedRecipeView : EntityView
    {
        public int AuthorId { get; set; }
        public int RecipeId { get; set; }
        public string? RecipeTitle { get; set; }
        public MealType MealType { get; set; }
        public string? Day { get; set; }
        public DateTime DateOfMeal { get; set; }
        public float Calories { get; set; }
    }
}
