using RecipeMvc.Aids.GoF.Crea;
namespace RecipeMvc.Tests.Aids.GoF.Crea;

[TestClass] public sealed class FactoryMethodTests : BaseTests {
    [TestMethod] public void CreateTest(){
        var x = new CopyTestClass1() { Id = 10001, Name = "Aaa Bbb Ccc", 
            ValidFrom = DateTime.Now };
        var y = FactoryMethod.Create<CopyTestClass2, CopyTestClass1>(x);
        isType(y, typeof(CopyTestClass2));
        equal("Aaa Bbb Ccc", y.Name);
        isNull(y.Id);
    }
}


