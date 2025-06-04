using RecipeMvc.Data.Entities;
namespace RecipeMvc.Domain;

public class Recipe(RecipeData? d) : Entity<RecipeData>(d)
{
    public int AuthorId => Data.AuthorId;
    public string Title => Data.Title;
    public string Description => Data.Description;
    public string? ImagePath => Data.ImagePath;
    public float Calories => Data.Calories;
    public string Tags => Data.Tags;
    internal List<RecipeIngredient> recipeIngredients = [];
    internal List<RecipeIngredient> Ingredient => recipeIngredients;
    public override async Task LoadLazy()
    {
        await base.LoadLazy();
        recipeIngredients.Clear();
        var ingredients = await getList<IRecipeIngredientRepo, RecipeIngredient>(nameof(RecipeIngredient.RecipeId), Id ?? 0);
        foreach (var i in ingredients)
        {
            if (i is null) continue;
            await i.LoadLazy();
            recipeIngredients.Add(i);
        }
    }
}
