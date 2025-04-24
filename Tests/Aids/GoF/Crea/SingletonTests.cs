using RecipeMvc.Aids.GoF.Crea;
namespace RecipeMvc.Tests.Aids.GoF.Crea;
[TestClass] public sealed class SingletonTests : BaseTests {

    [TestMethod] public void NewTest() => same(Singleton.New(), Singleton.New());
}



