using RecipeMvc.Data;

namespace RecipeMvc.Domain
{
    public class UserAccount(UserAccountData? d) : Entity<UserAccountData>(d)
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
