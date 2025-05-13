
    using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RecipeMvc.Data;
using RecipeMvc.Domain;

namespace RecipeMvc.Facade;

[DisplayName("MealPlan")]
public class MealPlanView : EntityView
{
    public DateTime Date { get; set; }

    // Kui koostad uut
    public List<MealPlanItemViewModel> Items { get; set; } = new();

    // Kui kuvad olemasolevat
    public List<PlannedRecipeView> PlannedItems { get; set; } = new();
}

