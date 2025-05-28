using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Soft.Data;
using RecipeMvc.Tests;

namespace RecipeMvc.Tests.Soft.Data; 
[TestClass] public class ApplicationDbContextTests :
    BaseClassTests<ApplicationDbContext, IdentityDbContext> {
   protected override ApplicationDbContext createObj() {
       var options = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseSqlite($"DataSource=file:TestDb_{Guid.NewGuid()};mode=memory;cache=shared")
          .Options;

       return new ApplicationDbContext(options);
   }
   [TestMethod] public void IngredientsTest() => isType(obj!.Ingredients, typeof(DbSet<IngredientData>));
   [TestMethod] public void UserAccountsTest() => isType(obj!.UserAccounts, typeof(DbSet<UserAccountData>));
   [TestMethod] public void PlannedRecipesTest() => isType(obj!.PlannedRecipes, typeof(DbSet<PlannedRecipeData>));
   [TestMethod] public void RecipesTest() => isType(obj!.Recipes, typeof(DbSet<RecipeData>));
   [TestMethod] public void ShoppingListsTest() => isType(obj!.ShoppingLists, typeof(DbSet<ShoppingListData>));
   [TestMethod] public void ShoppingListIngredientsTest() => isType(obj!.ShoppingListIngredients, typeof(DbSet<ShoppingListIngredientData>));
   [TestMethod] public void RecipeIngredientsTest() => isType(obj!.RecipeIngredients, typeof(DbSet<RecipeIngredientData>));
   [TestMethod] public void FavouritesTest() => isType(obj!.Favourites, typeof(DbSet<FavouriteData>));
}
