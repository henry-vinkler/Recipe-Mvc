using Microsoft.AspNetCore.Mvc;
using Mvc.Data;
using Mvc.Domain;
using Mvc.Facade;
using Mvc.Soft.Controllers;

namespace Mvc.Tests.Soft.Controllers;

[TestClass] public class BaseControllerTests() :
   ControllerBaseTests<BaseController<Movie, MovieData, MovieView>, Movie, MovieData, MovieView> {
   protected override BaseController<Movie, MovieData, MovieView> createObj() => new MoviesController(dbContext!);
    [TestMethod] public override void IsSealedTest() =>
        isFalse(typeof(BaseController<Movie, MovieData, MovieView>).IsSealed);
   [TestMethod] public void IsAbstractTest() =>
       isTrue(typeof(BaseController<Movie, MovieData, MovieView>).IsAbstract);
   [TestMethod] public override void IsBaseTypeOfTest() =>
       equal(typeof(BaseController<Movie, MovieData, MovieView>).BaseType, typeof(Controller));
    protected override Movie? createEntity(Func<MovieData> getData) => new (getData());
}
