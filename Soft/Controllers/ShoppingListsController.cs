using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data.Entities;
using System.Security.Claims;
using RecipeMvc.Data.DbContext;
using RecipeMvc.Facade.ShoppingList;


namespace RecipeMvc.Soft.Controllers;

[Authorize] public class ShoppingListsController : Controller
{
    private readonly ApplicationDbContext _db;
    public ShoppingListsController(ApplicationDbContext db) => _db = db;

    [AllowAnonymous]
    public async Task<IActionResult> Index(string? search)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var query = _db.ShoppingLists
            .Where(l => l.UserId == userId)
            .Include(l => l.Ingredients!)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(l => l.Name.ToLower().Contains(search.ToLower()));
        }

        var lists = await query.ToListAsync();

        var result = lists.Select(l => new ShoppingListView
        {
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
                IsChecked = i.IsChecked,
                IngredientName = i.Ingredient?.Name ?? "",
                Unit = i.Ingredient?.Unit ?? ""
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRecipeToShoppingList(int recipeId)
    {
        var recipeIngredients = await _db.RecipeIngredients
            .Where(ri => ri.RecipeId == recipeId)
            .ToListAsync();

        if (!recipeIngredients.Any())
        {
            TempData["Error"] = "Recipe not found or has no ingredients.";
            return RedirectToAction("Details", "Recipes", new { id = recipeId });
        }

        var recipe = await _db.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var shoppingList = new ShoppingListData
        {
            Name = recipe.Title,
            UserId = userId,
            IsChecked = false,
            Notes = "",
            Ingredients = recipeIngredients.Select(ri => new ShoppingListIngredientData
            {
                IngredientId = ri.IngredientId,
                Quantity = ri.Quantity.ToString(),
                IsChecked = false
            }).ToList()
        };

        _db.ShoppingLists.Add(shoppingList);
        await _db.SaveChangesAsync();

        return RedirectToAction("Details", new { id = shoppingList.Id });
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
