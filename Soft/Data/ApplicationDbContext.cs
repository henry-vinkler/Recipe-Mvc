using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;

namespace RecipeMvc.Soft.Data;

public class ApplicationDbContext : IdentityDbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<IngredientData> Ingredients { get; set; } = default!;
    public DbSet<UserAccountData>  UserAccounts { get; set; } = default!;
    public DbSet<RecipeIngredientData> RecipeIngredients { get; set; } = default!;
    public DbSet<FavouritesData> Favourites { get; set; } = default!;
}
