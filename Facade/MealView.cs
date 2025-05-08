
    using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RecipeMvc.Data;
using RecipeMvc.Domain;

namespace RecipeMvc.Facade;

[DisplayName("Meal")]
public class MealView : EntityView
{
    // [StringLength(40, MinimumLength = 3)]
    // [Required(ErrorMessage = "Name is required.")]
    // public string? Name { get; set; }

    [Required(ErrorMessage = "A note can not be longer than 100 charachters.")]
    [StringLength(100, MinimumLength = 0)]
    public string? Note { get; set; }
    public List<Week>? WeekDays { get; set; }
    public List<Meal>? Recipes { get; set; }

    public int? SelectedDayId { get; set; }
    public int? SelectedRecipeId { get; set; }
}

