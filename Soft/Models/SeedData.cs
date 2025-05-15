using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Models;

public static class SeedData {
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {
            if (!context.Recipes.Any())
            {
                return;
            }
            var user = new UserAccountData
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Username = "johnny",
                Password = "secret"
            };
            context.UserAccounts.Add(user);
            context.SaveChanges();

            context.Recipes.AddRange(
                new RecipeData
                {
                    AuthorId = user.Id,
                    Title = "Delicious pasta with tomato sauce",
                    Description = "Ready in 25 minutes. It’s a simple yet satisfying meal that brings warmth to every bite.",
                    ImagePath = "pasta.jpg",
                    Calories = 300f,
                    Tags = "Italian, Pasta"
                },
                new RecipeData
                {
                    AuthorId = user.Id,
                    Title = "Fresh salad with vegetables",
                    Description = "Ready in 10 minutes. It’s refreshing, healthy, and perfect as a side or a light main course.",
                    ImagePath = "salad.jpg",
                    Calories = 150f,
                    Tags = "Healthy, Salad"
                },
                new RecipeData
                {
                    AuthorId = user.Id,
                    Title = "Cheesy pizza with pepperoni",
                    Description = "Ready in 30 minutes. Ideal for sharing—or not!",
                    ImagePath = "pizza.jpg",
                    Calories = 400f,
                    Tags = "Italian, Pizza"
                },
                new RecipeData
                {
                    AuthorId = user.Id,
                    Title = "Juicy burger with fries",
                    Description = "Ready in 35 minutes. It’s indulgent, satisfying, and packed with bold, comforting flavors.",
                    ImagePath = "burger.jpg",
                    Calories = 500f,
                    Tags = "Fast Food, Burger"
                });
            context.SaveChanges();


            if (!context.Ingredients.Any())
            {
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


            if (!context.RecipeIngredients.Any())
            {
               
                var pasta = context.Recipes.Single(r => r.Title == "Pasta");
                var salad = context.Recipes.Single(r => r.Title == "Salad");
                var pizza = context.Recipes.Single(r => r.Title == "Pizza");
                var burger = context.Recipes.Single(r => r.Title == "Burger");

                var sauce = context.Ingredients.Single(i => i.Name == "Tomato Sauce");
                var noodles = context.Ingredients.Single(i => i.Name == "Pasta Noodles");
                var lettuce = context.Ingredients.Single(i => i.Name == "Lettuce");
                var oil = context.Ingredients.Single(i => i.Name == "Olive Oil");
                var cheese = context.Ingredients.Single(i => i.Name == "Cheese");
                var pepper = context.Ingredients.Single(i => i.Name == "Pepperoni");
                var bun = context.Ingredients.Single(i => i.Name == "Bun");
                var patty = context.Ingredients.Single(i => i.Name == "Patty");
                var fries = context.Ingredients.Single(i => i.Name == "Fries");

                context.RecipeIngredients.AddRange(
                  
                    new RecipeIngredientData { RecipeId = pasta.Id, IngredientId = noodles.Id, Quantity = 200f },
                    new RecipeIngredientData { RecipeId = pasta.Id, IngredientId = sauce.Id, Quantity = 150f },
                    new RecipeIngredientData { RecipeId = pasta.Id, IngredientId = oil.Id, Quantity = 10f },

                   
                    new RecipeIngredientData { RecipeId = salad.Id, IngredientId = lettuce.Id, Quantity = 100f },
                    new RecipeIngredientData { RecipeId = salad.Id, IngredientId = oil.Id, Quantity = 20f },

                 
                    new RecipeIngredientData { RecipeId = pizza.Id, IngredientId = noodles.Id, Quantity = 100f },
                    new RecipeIngredientData { RecipeId = pizza.Id, IngredientId = cheese.Id, Quantity = 100f },
                    new RecipeIngredientData { RecipeId = pizza.Id, IngredientId = sauce.Id, Quantity = 50f },
                    new RecipeIngredientData { RecipeId = pizza.Id, IngredientId = pepper.Id, Quantity = 20f },

                  
                    new RecipeIngredientData { RecipeId = burger.Id, IngredientId = bun.Id, Quantity = 1f },
                    new RecipeIngredientData { RecipeId = burger.Id, IngredientId = patty.Id, Quantity = 1f },
                    new RecipeIngredientData { RecipeId = burger.Id, IngredientId = fries.Id, Quantity = 50f }
                );
                context.SaveChanges();
            }
        }

    }
}
