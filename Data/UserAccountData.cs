namespace RecipeMvc.Data
{
    public sealed class UserAccountData : EntityData<UserAccountData>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<RecipeData> Recipes { get; set; } = new List<RecipeData>();

    }
}
