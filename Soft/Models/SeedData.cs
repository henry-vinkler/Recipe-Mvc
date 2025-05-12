using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeMvc.Data;
using RecipeMvc.Soft.Data;
using System;
using System.Linq;

namespace RecipeMvc.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {
            if (context.Recipes.Any())
            {
                return;   
            }
            context.Recipes.AddRange(
                new RecipeData
                {
                    Title = "Delicious pasta with tomato sauce",
                    Description = "Ready in 25 minutes\r\n Ingredients: Pasta (spaghetti or penne), ripe tomatoes, garlic, onion, olive oil, basil, salt, pepper, Parmesan cheese\r\n This classic Italian dish features perfectly cooked pasta coated in a rich, homemade tomato sauce. The sauce is simmered with garlic and herbs for a deep, comforting flavor. Finished with freshly grated Parmesan, it’s a simple yet satisfying meal that brings warmth to every bite.",
                    ImagePath = "pasta.jpg",
                    Calories = 300f,
                    Tags = "Italian, Pasta"
                },
                new RecipeData
                {
                    Title = "Fresh salad with vegetables",
                    Description = "Ready in 10 minutes\r\n Ingredients: Lettuce, cucumber, cherry tomatoes, bell pepper, red onion, olive oil, lemon juice, salt, pepper\r\nA vibrant and crunchy salad made with the freshest seasonal vegetables. Lightly dressed with olive oil and lemon juice, it enhances the natural flavors without overpowering them. It’s refreshing, healthy, and perfect as a side or a light main course.",
                    ImagePath = "salad.jpg",
                    Calories = 150f,
                    Tags = "Healthy, Salad"
                },
                new RecipeData
                {
                    Title = "Cheesy pizza with pepperoni",
                    Description = "Ready in 30 minutes (plus dough prep if homemade)\r\n Ingredients: Pizza dough, tomato sauce, mozzarella cheese, pepperoni, oregano, olive oil\r\nThis pizza boasts a golden, crispy crust topped with tangy tomato sauce, gooey melted mozzarella, and spicy pepperoni slices. Baked to perfection, it delivers a bold, cheesy, and savory experience in every bite. Ideal for sharing—or not!",
                    ImagePath = "pizza.jpg",
                    Calories = 400f,
                    Tags = "Italian, Pizza"
                },
                new RecipeData
                {
                    Title = "Juicy burger with fries",
                    Description = "Ready in 35 minutes\r\n Ingredients: Ground beef, burger buns, lettuce, tomato, cheese, pickles, burger sauce, potatoes, salt, oil\r\nA hearty burger made with a juicy beef patty, fresh toppings, and a flavorful sauce, all nestled in a soft bun. Served with crispy golden fries, this combo is a timeless favorite. It’s indulgent, satisfying, and packed with bold, comforting flavors.",
                    ImagePath = "burger.jpg",
                    Calories = 500f,
                    Tags = "Fast Food, Burger"
                }
            );
            context.SaveChanges();
        }
    }
}
