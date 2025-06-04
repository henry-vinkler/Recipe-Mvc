using RecipeMvc.Core;

namespace RecipeMvc.Domain;
public interface IRecipeIngredientRepo : IRepo<RecipeIngredient>;
public interface IRecipeRepo : IRepo<Recipe>;
public interface IIngredientRepo : IRepo<Ingredient>;
public interface IUserAccountRepo : IRepo<UserAccount>
{
    Task<UserAccount?> GetByUsernameOrEmailAsync(string usernameOrEmail);
    Task<bool> ExistsAsync(string? username, string? email, int? excludeUserId = null);
}