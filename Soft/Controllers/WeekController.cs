using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Data;

namespace RecipeMvc.Soft.Controllers;
public class WeekController(ApplicationDbContext c)
    : BaseController<Week, WeekData, WeekView>(c, new WeekViewFactory(), d => new (d)) { }
