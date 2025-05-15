using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeMvc.Data;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            if (context.Ingredients.Any())
            {
                return; // DB has been seeded
            }

            context.Ingredients.AddRange(
                new IngredientData { Name = "Flour", Calories = 364, Unit = "g" },
                new IngredientData { Name = "Sugar", Calories = 387, Unit = "g" },
                new IngredientData { Name = "Butter", Calories = 717, Unit = "g" },
                new IngredientData { Name = "Egg", Calories = 155, Unit = "g" },
                new IngredientData { Name = "Milk", Calories = 42, Unit = "ml" },
                new IngredientData { Name = "Salt", Calories = 0, Unit = "g" },
                new IngredientData { Name = "Olive Oil", Calories = 884, Unit = "g" },
                new IngredientData { Name = "Chicken Breast", Calories = 165, Unit = "g" },
                new IngredientData { Name = "Tomato", Calories = 18, Unit = "g" },
                new IngredientData { Name = "Onion", Calories = 40, Unit = "g" },
                new IngredientData { Name = "Garlic", Calories = 149, Unit = "g" },
                new IngredientData { Name = "Carrot", Calories = 41, Unit = "g" },
                new IngredientData { Name = "Potato", Calories = 77, Unit = "g" },
                new IngredientData { Name = "Rice", Calories = 130, Unit = "g" },
                new IngredientData { Name = "Pasta", Calories = 131, Unit = "g" },
                new IngredientData { Name = "Cheese", Calories = 402, Unit = "g" },
                new IngredientData { Name = "Beef", Calories = 250, Unit = "g" },
                new IngredientData { Name = "Pork", Calories = 242, Unit = "g" },
                new IngredientData { Name = "Apple", Calories = 52, Unit = "g" },
                new IngredientData { Name = "Banana", Calories = 89, Unit = "g" },
                new IngredientData { Name = "Lettuce", Calories = 15, Unit = "g" },
                new IngredientData { Name = "Cucumber", Calories = 16, Unit = "g" },
                new IngredientData { Name = "Bell Pepper", Calories = 31, Unit = "g" },
                new IngredientData { Name = "Broccoli", Calories = 34, Unit = "g" },
                new IngredientData { Name = "Spinach", Calories = 23, Unit = "g" },
                new IngredientData { Name = "Water", Calories = 0, Unit = "ml" },
                new IngredientData { Name = "Vegetable Oil", Calories = 8.84f, Unit = "ml" },
                new IngredientData { Name = "Soy Sauce", Calories = 0.53f, Unit = "ml" },
                new IngredientData { Name = "Vinegar", Calories = 0.21f, Unit = "ml" },
                new IngredientData { Name = "Lemon Juice", Calories = 0.22f, Unit = "ml" },
                new IngredientData { Name = "Honey", Calories = 3.04f, Unit = "ml" },
                new IngredientData { Name = "Ketchup", Calories = 1.12f, Unit = "ml" },
                new IngredientData { Name = "Mayonnaise", Calories = 6.8f, Unit = "ml" },
                new IngredientData { Name = "Coconut Milk", Calories = 1.97f, Unit = "ml" },
                new IngredientData { Name = "Cream", Calories = 3.45f, Unit = "ml" }
            );
            context.SaveChanges();
        }
    }
}
