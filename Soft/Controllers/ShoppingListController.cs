using Microsoft.AspNetCore.Mvc;

namespace RecipeMvc.Soft.Controllers
{
    public class ShoppingListController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Load favorites and added recipes from the database
            return View();
        }

        [HttpPost]
        public IActionResult CreateListFromMealweek()
        {
            // TODO: Implement functionality for creating a shopping list from the meal week
            return Content("Functionality to be added.");
        }
    }
}