using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;

namespace RecipeMvc.Soft.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<IngredientData> Ingredients { get; set; } = default!;
    public DbSet<UserAccountData> UserAccounts { get; set; } = default!;
    public DbSet<MealData> Meal { get; set; } = default!;
    public DbSet<RecipeData> Recipes { get; set; } = default!;
    public DbSet<ShoppingListIngredientData> ShoppingListIngredients { get; set; } = default!;
    public DbSet<RecipeIngredientData> RecipeIngredients { get; set; } = default!;
    public DbSet<FavouriteData> Favourites { get; set; } = default!;
}
