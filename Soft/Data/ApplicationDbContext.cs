using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Domain;

namespace RecipeMvc.Soft.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<IngredientData> Ingredients { get; set; } = default!;
    public DbSet<UserAccountData> UserAccounts { get; set; } = default!;
    public DbSet<MealPlanData> Meal { get; set; } = default!;//Nädala toit
    public DbSet<WeekData> Weeks { get; set; }= default!;//Nädala toit
    public DbSet<MealPlanData> MealWeeks { get; set; }= default!;//Nädala toit
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MealPlanData>()
            .HasOne(m => m.Recipe)
            .WithOne()
            .HasForeignKey<MealPlanData>(m => m.RecipeId)
            .OnDelete(DeleteBehavior.Restrict);
    }


    public DbSet<RecipeData> Recipes { get; set; } = default!;
    public DbSet<ShoppingListIngredientData> ShoppingListIngredients { get; set; } = default!;
}
