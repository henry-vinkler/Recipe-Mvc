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
                return ; // DB has been seeded
            }

            context.Ingredients.AddRange(
                new IngredientData { Name = "Flour", Calories = 3.64f, Unit = "g" },
                new IngredientData { Name = "Sugar", Calories = 3.87f, Unit = "g" },
                new IngredientData { Name = "Butter", Calories = 7.17f, Unit = "g" },
                new IngredientData { Name = "Egg", Calories = 1.55f, Unit = "g" },
                new IngredientData { Name = "Milk", Calories = 0.42f, Unit = "ml" },
                new IngredientData { Name = "Salt", Calories = 0f, Unit = "g" },
                new IngredientData { Name = "Olive Oil", Calories = 8.84f, Unit = "g" },
                new IngredientData { Name = "Chicken Breast", Calories = 1.65f, Unit = "g" },
                new IngredientData { Name = "Tomato", Calories = 0.18f, Unit = "g" },
                new IngredientData { Name = "Onion", Calories = 0.4f, Unit = "g" },
                new IngredientData { Name = "Garlic", Calories = 1.49f, Unit = "g" },
                new IngredientData { Name = "Carrot", Calories = 0.41f, Unit = "g" },
                new IngredientData { Name = "Potato", Calories = 0.77f, Unit = "g" },
                new IngredientData { Name = "Rice", Calories = 1.3f, Unit = "g" },
                new IngredientData { Name = "Pasta", Calories = 1.31f, Unit = "g" },
                new IngredientData { Name = "Cheese", Calories = 4.02f, Unit = "g" },
                new IngredientData { Name = "Beef", Calories = 2.5f, Unit = "g" },
                new IngredientData { Name = "Pork", Calories = 2.42f, Unit = "g" },
                new IngredientData { Name = "Apple", Calories = 0.52f, Unit = "g" },
                new IngredientData { Name = "Banana", Calories = 0.89f, Unit = "g" },
                new IngredientData { Name = "Lettuce", Calories = 0.15f, Unit = "g" },
                new IngredientData { Name = "Cucumber", Calories = 0.16f, Unit = "g" },
                new IngredientData { Name = "Bell Pepper", Calories = 0.31f, Unit = "g" },
                new IngredientData { Name = "Broccoli", Calories = 0.34f, Unit = "g" },
                new IngredientData { Name = "Spinach", Calories = 0.23f, Unit = "g" },
                new IngredientData { Name = "Water", Calories = 0f, Unit = "ml" },
                new IngredientData { Name = "Vegetable Oil", Calories = 0.0884f, Unit = "ml" },
                new IngredientData { Name = "Soy Sauce", Calories = 0.0053f, Unit = "ml" },
                new IngredientData { Name = "Vinegar", Calories = 0.0021f, Unit = "ml" },
                new IngredientData { Name = "Lemon Juice", Calories = 0.0022f, Unit = "ml" },
                new IngredientData { Name = "Honey", Calories = 0.0304f, Unit = "ml" },
                new IngredientData { Name = "Ketchup", Calories = 0.0112f, Unit = "ml" },
                new IngredientData { Name = "Mayonnaise", Calories = 0.068f, Unit = "ml" },
                new IngredientData { Name = "Coconut Milk", Calories = 0.0197f, Unit = "ml" },
                new IngredientData { Name = "Cream", Calories = 0.0345f, Unit = "ml" }
            );
            context.MealPlans.AddRange(
                new MealPlanData { DateOfMeal = DateTime.Now, UserId = 1, Note = "Test meal plan" },
                new MealPlanData { DateOfMeal = DateTime.Now.AddDays(1), UserId = 1, Note = "Another test meal plan" }
            );
            if (context is null) return;
            if (context.Recipes.Any()) return;

            if (!context.Recipes.Any())
            {
                
                var recipes = new RecipeData[] {
                new() { Title = "Pasta", Description = "Delicious pasta with tomato sauce",Tags = "Pasta, Tomato sauce", Calories = 500,AuthorId = 1 },
                new() { Title = "Salad", Description = "Fresh salad with vegetables", Tags = "Salad, Vegetables", Calories = 200, AuthorId = 1 },
                new() { Title = "Pizza", Description = "Cheesy pizza with pepperoni", Tags = "Pizza, Cheese, Pepperoni", Calories = 800, AuthorId = 1 },
                new() { Title = "Burger", Description = "Juicy burger with lettuce and tomato", Tags = "Burger, Lettuce, Tomato", Calories = 600,   AuthorId = 1 },
                new() { Title = "Sushi", Description = "Sushi rolls with fish and rice", Tags = "Sushi, Fish, Rice", Calories = 300, AuthorId = 1 },
                new() { Title = "Tacos", Description = "Spicy tacos with beef and salsa", Tags = "Tacos, Beef, Salsa", Calories = 400, AuthorId = 1 },
                new() { Title = "Ice Cream", Description = "Creamy ice cream with chocolate", Tags = "Ice Cream, Chocolate", Calories = 250, AuthorId = 1 },
                };
                context.Recipes.AddRange(recipes);
            }

        
        if (!context.MealPlans.Any())
        {
            context.MealPlans.AddRange(
                new MealPlanData { DateOfMeal = DateTime.Now, UserId = 1, Note = "Test meal plan" },
                new MealPlanData { DateOfMeal = DateTime.Now.AddDays(1), UserId = 1, Note = "Another test meal plan" }
            );
        }
            
            context.SaveChanges();
        }
    }
}
