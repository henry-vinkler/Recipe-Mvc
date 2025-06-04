using RecipeMvc.Data.Entities;

namespace RecipeMvc.Domain
{
    public class UserAccount(UserAccountData? d) : Entity<UserAccountData>(d)
    {
        private void Set<T>(Action<UserAccountData, T> setter, T value)
        {
            if (Data != null) setter(Data, value);
        }

        public string FirstName
        {
            get => Data?.FirstName ?? string.Empty;
            set => Set((d, v) => d.FirstName = v, value);
        }
        public string LastName
        {
            get => Data?.LastName ?? string.Empty;
            set => Set((d, v) => d.LastName = v, value);
        }
        public string Email
        {
            get => Data?.Email ?? string.Empty;
            set => Set((d, v) => d.Email = v, value);
        }
        public string Username
        {
            get => Data?.Username ?? string.Empty;
            set => Set((d, v) => d.Username = v, value);
        }
        public string Password
        {
            get => Data?.Password ?? string.Empty;
            set => Set((d, v) => d.Password = v, value);
        }

        // Domain collection for related recipes
        internal List<Recipe> userRecipes = [];
        public IReadOnlyList<Recipe> Recipes => userRecipes;

        public override async Task LoadLazy()
        {
            await base.LoadLazy();
            userRecipes.Clear();
            var recipes = await getList<IRecipeRepo, Recipe>(nameof(RecipeData.AuthorId), Id ?? 0);
            foreach (var r in recipes)
            {
                if (r is null) continue;
                await r.LoadLazy();
                userRecipes.Add(r);
            }
        }
    }
}