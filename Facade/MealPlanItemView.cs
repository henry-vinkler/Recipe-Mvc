using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeMvc.Aids;

public class MealPlanItemViewModel
    {
        public MealType MealType { get; set; }
        public int? SelectedRecipeId { get; set; }
        public IEnumerable<SelectListItem> AvailableRecipes { get; set; }
    }