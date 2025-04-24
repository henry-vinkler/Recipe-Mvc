using RecipeMvc.Data;
namespace RecipeMvc.Domain;
public class Recipe(RecipeData? d) : Entity<RecipeData>(d)
{
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public float Calories { get; set; }
    public string Tags { get; set; }
}
