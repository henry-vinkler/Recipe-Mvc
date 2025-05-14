using System.ComponentModel.DataAnnotations;
namespace RecipeMvc.Facade.Account;

public class RegistrationView : EditUserAccountView
{
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, MinimumLength =5, ErrorMessage = "Password must be between 5 and 20 characters long.")]
    [RegularExpression(@"^(?=.*\d).+$", ErrorMessage = "Password must contain at least one number.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    public required string ConfirmPassword { get; set; }
}

