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
                    new IngredientData { Name = "Tomato Sauce", Unit = "ml" },
                    new IngredientData { Name = "Pasta Noodles", Unit = "g" },
                    new IngredientData { Name = "Lettuce", Unit = "g" },
                    new IngredientData { Name = "Olive Oil", Unit = "ml" },
                    new IngredientData { Name = "Cheese", Unit = "g" },
                    new IngredientData { Name = "Pepperoni", Unit = "slices" },
                    new IngredientData { Name = "Bun", Unit = "pcs" },
                    new IngredientData { Name = "Patty", Unit = "pcs" },
                    new IngredientData { Name = "Fries", Unit = "g" }
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
