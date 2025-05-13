using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;

namespace RecipeMvc.Soft.Data;

public class DbInitializer(ApplicationDbContext? c)
{
    private int count;
    private int size;
    public async Task Initialize(int itemsCount = 1000, int listSize = 250) {
        count = itemsCount;
        size = listSize;
        if (c is null) return;
        c.Database.EnsureCreated();
        foreach (var set in sets)
        {
            var method = methodInfo(set);
            if (method is null) continue;
            await (Task)method.Invoke(this, [set])!;
        }
    }
    private MethodInfo? methodInfo(object? set)
    {
        var t = set?.GetType().GetGenericArguments().FirstOrDefault();
        if (t == null) return null;
        return typeof(DbInitializer)
            .GetMethod(nameof(seedData), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(t);
    }
    private IEnumerable<object?> sets
    {
        get
        {
            var t = c?.GetType();
            var props = t?.GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly
            );
            var dbProps = props?.Where(p => p.PropertyType.IsGenericType &&
                p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
            var setObjects = dbProps?.Select(p => p.GetValue(c));
            var notNull = setObjects?.Where(p => p is not null);
            var result = notNull?.ToArray() ?? [];
            return result;
        }
    }
    private async Task seedData<TEntity>(DbSet<TEntity> set)
       where TEntity : EntityData, new()
    {
        var cnt = set.Count();
        var list = new List<TEntity>(size);
        foreach (var d in getData<TEntity>(count - cnt))
        {
            list.Add(d);
            Thread.Sleep(1);
            if (list.Count < size) continue;
            await save(set, list);
        }
        await save(set, list);
    }
    private IEnumerable<TEntity> getData<TEntity>(int cnt) where TEntity : EntityData, new()
    {
        for (var i = 0; i < cnt; i++)
        {
            var d = Aids.Random.Object<TEntity>();
            if (d is null) continue;
            d.Id = 0;
            yield return d;
        }
    }
    private async Task save<TEntity>(DbSet<TEntity> set, List<TEntity> list) where TEntity : EntityData, new()
    {
        if (c is not null)
        {
            set.AddRange(list);
            await c.SaveChangesAsync();
        }
        list.Clear();
    }
    public void Initialize()
    {
        if (c is null) return;
        if (c.Recipes.Any()) return;

        var recipes = new RecipeData[] {
            new() { Title = "Pasta", Description = "Delicious pasta with tomato sauce",Tags = "Pasta, Tomato sauce", Calories = 500,AuthorId = 1 },
            new() { Title = "Salad", Description = "Fresh salad with vegetables", Tags = "Salad, Vegetables", Calories = 200, AuthorId = 1 },
            new() { Title = "Pizza", Description = "Cheesy pizza with pepperoni", Tags = "Pizza, Cheese, Pepperoni", Calories = 800, AuthorId = 1 },
            new() { Title = "Burger", Description = "Juicy burger with lettuce and tomato", Tags = "Burger, Lettuce, Tomato", Calories = 600,   AuthorId = 1 },
            new() { Title = "Sushi", Description = "Sushi rolls with fish and rice", Tags = "Sushi, Fish, Rice", Calories = 300, AuthorId = 1 },
            new() { Title = "Tacos", Description = "Spicy tacos with beef and salsa", Tags = "Tacos, Beef, Salsa", Calories = 400, AuthorId = 1 },
            new() { Title = "Ice Cream", Description = "Creamy ice cream with chocolate", Tags = "Ice Cream, Chocolate", Calories = 250, AuthorId = 1 },
        };
            // public int AuthorId { get; set; }
            // public string Title { get; set; }
            // public string Description { get; set; }
            // public string ImagePath { get; set; }
            // public float Calories { get; set; }
            // public string Tags { get; set; }
    }
}
