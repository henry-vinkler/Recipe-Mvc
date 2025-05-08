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
                    Title = "Pasta",
                    Description = "Delicious pasta with tomato sauce",
                    ImagePath = "pasta.jpg",
                    Calories = 300f,
                    Tags = "Italian, Pasta"
                },
                new RecipeData
                {
                    Title = "Salad",
                    Description = "Fresh salad with vegetables",
                    ImagePath = "salad.jpg",
                    Calories = 150f,
                    Tags = "Healthy, Salad"
                },
                new RecipeData
                {
                    Title = "Pizza",
                    Description = "Cheesy pizza with pepperoni",
                    ImagePath = "pizza.jpg",
                    Calories = 400f,
                    Tags = "Italian, Pizza"
                },
                new RecipeData
                {
                    Title = "Burger",
                    Description = "Juicy burger with fries",
                    ImagePath = "burger.jpg",
                    Calories = 500f,
                    Tags = "Fast Food, Burger"
                }
            );
            context.SaveChanges();
        }
    }
}
