using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeMvc.Facade;
[DisplayName("Recipe author")] public class RecipeAuthorView : EntityView {
    [Display(Name = "Recipe")] public int RecipeId { get; set; }
    [Display(Name = "User")] public int UserAccountId { get; set; }
    [Display(Name = "Recipe")] public string? Recipe { get; set; }
    [Display(Name = "User")] public string? UserAccount { get; set; }
}
