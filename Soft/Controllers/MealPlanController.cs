using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Soft.Controllers;

public class MealController(ApplicationDbContext c)
    : BaseController<MealPlan, MealPlanData, MealPlanView>(c, new MealPlanViewFactory(), d => new(d))
{
  
}

