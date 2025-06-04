using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;

namespace RecipeMvc.Soft.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<IngredientData> Ingredients { get; set; } = default!;
    public DbSet<UserAccountData> UserAccounts { get; set; } = default!;
    public DbSet<PlannedRecipeData> PlannedRecipes { get; set; }= default!;
    public DbSet<RecipeData> Recipes { get; set; } = default!;
    public DbSet<ShoppingListData> ShoppingLists { get; set; } = default!;
    public DbSet<ShoppingListIngredientData> ShoppingListIngredients { get; set; } = default!;
    public DbSet<RecipeIngredientData> RecipeIngredients { get; set; } = default!;
    public DbSet<FavouriteData> Favourites { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ShoppingListIngredientData>()
            .HasOne(i => i.Ingredient)
            .WithMany()
            .HasForeignKey(i => i.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RecipeIngredientData>()
       .HasKey(ri => ri.Id); 

        modelBuilder.Entity<RecipeIngredientData>()
            .HasOne<RecipeData>()
            .WithMany()
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeIngredientData>()
            .HasOne<IngredientData>()
            .WithMany()
            .HasForeignKey(ri => ri.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}

