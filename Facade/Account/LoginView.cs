using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeMvc.Facade.Account;

public class LoginView
{
    [Required(ErrorMessage = "Username or Email is required.")]
    [MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
    [DisplayName("Username or Email")]
    public required string UsernameOrEmail { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 20 characters long.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}

