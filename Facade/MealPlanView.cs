
    using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RecipeMvc.Data;
using RecipeMvc.Domain;

namespace RecipeMvc.Facade;

[DisplayName("MealPlan")]
public class MealPlanView : EntityView
{
    [DisplayName("Date")]public DateTime Date { get; set; }
    [DisplayName("Recipes")]public List<MealPlanItemViewModel> Items { get; set; }
}

