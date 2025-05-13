using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Models;

public static class SeedData {
    public static void Initialize(IServiceProvider serviceProvider) {
        using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>())) {
            if (context.Recipes.Any()) {
                return;   
            }
            var user = new UserAccountData {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Username = "johnny",
                Password = "secret"
            };
            context.UserAccounts.Add(user);
            context.SaveChanges();

            context.Recipes.AddRange(
                new RecipeData {
                    AuthorId = 1,
                    Title = "Delicious pasta with tomato sauce",
                    Description = "Ready in 25 minutes. It’s a simple yet satisfying meal that brings warmth to every bite.",
                    ImagePath = "pasta.jpg",
                    Calories = 300f,
                    Tags = "Italian, Pasta"
                },
                new RecipeData {
                    AuthorId = 2,
                    Title = "Fresh salad with vegetables",
                    Description = "Ready in 10 minutes. It’s refreshing, healthy, and perfect as a side or a light main course.",
                    ImagePath = "salad.jpg",
                    Calories = 150f,
                    Tags = "Healthy, Salad"
                },
                new RecipeData {
                    AuthorId = 3,
                    Title = "Cheesy pizza with pepperoni",
                    Description = "Ready in 30 minutes. Ideal for sharing—or not!",
                    ImagePath = "pizza.jpg",
                    Calories = 400f,
                    Tags = "Italian, Pizza"
                },
                new RecipeData {
                    AuthorId = 4,
                    Title = "Juicy burger with fries",
                    Description = "Ready in 35 minutes. It’s indulgent, satisfying, and packed with bold, comforting flavors.",
                    ImagePath = "burger.jpg",
                    Calories = 500f,
                    Tags = "Fast Food, Burger"
                });
            context.SaveChanges();
        }
    }
}
