using Mvc.Data;
using Mvc.Domain;
using Mvc.Facade;
using Mvc.Soft.Controllers;

namespace Mvc.Tests.Soft.Controllers;

[TestClass] public class TestingControllerTests() :
   ControllerBaseTests<TestingController, Testing, TestingData, TestingView> {
    protected override Testing? createEntity(Func<TestingData> getData)
       => new (getData());

    protected override TestingController createObj() => new(dbContext!);
}
