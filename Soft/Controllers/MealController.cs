using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Soft.Controllers;
public class MealController(ApplicationDbContext c)
    : BaseController<Meal, MealData, MealView>(c, new MealViewFactory(), d => new (d)) { }
