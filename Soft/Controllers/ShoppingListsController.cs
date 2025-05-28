using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Soft.Data;
using RecipeMvc.Facade;
using System.Security.Claims;


namespace RecipeMvc.Soft.Controllers;

[Authorize] public class ShoppingListsController : Controller
{
    private readonly ApplicationDbContext _db;
    public ShoppingListsController(ApplicationDbContext db) => _db = db;

    [AllowAnonymous]
    public async Task<IActionResult> Index() {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var lists = await _db.ShoppingLists
            .Where(l => l.UserId == userId)
            .Include(l => l.Ingredients!)
            .ToListAsync();

        var result = lists.Select(l => new ShoppingListView {
            Id = l.Id,
            Name = l.Name,
            Notes = l.Notes,
            IsChecked = l.IsChecked,
            UserId = l.UserId
        }).ToList();

        return View(result);
    }

    public async Task<IActionResult> Details(int? id) {
        if (id is null) return NotFound();

        var list = await _db.ShoppingLists
    .Include(l => l.Ingredients!)
        .ThenInclude(i => i.Ingredient)
    .FirstOrDefaultAsync(l => l.Id == id);


        if (list == null) return NotFound();

        var model = new ShoppingListView
        {
            Id = list.Id,
            Name = list.Name,
            Notes = list.Notes,
            IsChecked = list.IsChecked,
            UserId = list.UserId,
            Ingredients = list.Ingredients.Select(i => new ShoppingListIngredientView
            {
                IngredientId = i.IngredientId,
                Quantity = i.Quantity,
                IsChecked = i.IsChecked
            }).ToList()
        };

        return View(model);
    }
    public IActionResult Create() => View(new ShoppingListView());

    [HttpPost] [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShoppingListView model) {
        if (!ModelState.IsValid) return View(model);

        model.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var entity = new ShoppingListData
        {
            Name = model.Name,
            Notes = model.Notes,
            IsChecked = model.IsChecked,
            UserId = model.UserId
        };

        _db.ShoppingLists.Add(entity);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id) {
        if (id is null) return NotFound();
        var list = await _db.ShoppingLists.FindAsync(id);
        if (list == null) return NotFound();

        return View(new ShoppingListView
        {
            Id = list.Id,
            Name = list.Name,
            Notes = list.Notes,
            IsChecked = list.IsChecked,
            UserId = list.UserId
        });
    }

    [HttpPost] [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ShoppingListView model) {
        if (!ModelState.IsValid) return View(model);

        var entity = await _db.ShoppingLists.FindAsync(model.Id);
        if (entity == null) return NotFound();

        entity.Name = model.Name;
        entity.Notes = model.Notes;
        entity.IsChecked = model.IsChecked;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id) {
        if (id is null) return NotFound();
        var list = await _db.ShoppingLists.FindAsync(id);
        if (list == null) return NotFound();

        return View(new ShoppingListView
        {
            Id = list.Id,
            Name = list.Name,
            Notes = list.Notes,
            IsChecked = list.IsChecked,
            UserId = list.UserId
        });
    }

    [HttpPost, ActionName("Delete")] [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) {
        var list = await _db.ShoppingLists
            .Include(l => l.Ingredients)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (list != null)
        {
            _db.ShoppingListIngredients.RemoveRange(list.Ingredients);
            _db.ShoppingLists.Remove(list);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost] [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRecipeToShoppingList(int recipeId)
    {
        var recipe = await _db.Recipes
            .Include(r => r.RecipeIngredients!)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == recipeId);

        if (recipe == null || recipe.RecipeIngredients == null || !recipe.RecipeIngredients.Any())
            return NotFound();

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var shoppingList = new ShoppingListData
        {
            Name = $"From the recipe: {recipe.Title}",
            UserId = userId,
            IsChecked = false,
            Notes = "",
            Ingredients = recipe.RecipeIngredients.Select(ri => new ShoppingListIngredientData
            {
                IngredientId = ri.IngredientId,
                Quantity = ri.Quantity.ToString(),
                IsChecked = false
            }).ToList()
        };

        _db.ShoppingLists.Add(shoppingList);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = shoppingList.Id });
    }

    [HttpPost] [ValidateAntiForgeryToken]

        public async Task<IActionResult> UpdateCheckedStatus(int ShoppingListId, List<ShoppingListIngredientView> Ingredients)
        {
            var list = await _db.ShoppingLists
                .Include(l => l.Ingredients)
                .FirstOrDefaultAsync(l => l.Id == ShoppingListId);

            if (list == null) return NotFound();

            foreach (var item in Ingredients)
            {
                var target = list.Ingredients.FirstOrDefault(i => i.IngredientId == item.IngredientId);
                if (target != null)
                {
                    target.IsChecked = item.IsChecked;
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = ShoppingListId });
        }
        

}
