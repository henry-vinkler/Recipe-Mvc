using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Facade;
using RecipeMvc.Soft.Controllers;

namespace RecipeMvc.Tests.Soft.Controllers;

[TestClass] public class TestingControllerTests() :
   ControllerBaseTests<TestingController, Testing, TestingData, TestingView> {
    protected override Testing? createEntity(Func<TestingData> getData)
       => new (getData());

    protected override TestingController createObj() => new(dbContext!);
}
