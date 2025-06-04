using RecipeMvc.Core;

namespace RecipeMvc.Domain;
public interface IRecipeIngredientRepo : IRepo<RecipeIngredient>;
public interface IRecipeRepo : IRepo<Recipe>;
public interface IIngredientRepo : IRepo<Ingredient>;
public interface IUserAccountRepo : IRepo<UserAccount>;