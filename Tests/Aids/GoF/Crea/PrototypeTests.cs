using MvcTunnikontroll.Aids.GoF.Crea;
using MvcTunnikontroll.Tests;

namespace MvcTunnikontroll.Tests.Aids.GoF.Crea;

internal class OtherTestClass {
    public int? Value { get; set; }
}
internal class TestClass: ICloneable<TestClass> {
    public TestClass Clone() {
        var x = MemberwiseClone() as TestClass;
        if (x is null) return new TestClass();
        if (Class != null) x.Class = Class?.Clone();
        if (OtherClass != null) x.OtherClass = new (){ Value = OtherClass?.Value};
        return x;
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? ValidFrom { get; set; }
    public TestClass? Class { get; set; }
    public OtherTestClass? OtherClass { get; set; }
}

[TestClass] public sealed class PrototypeTests : ClassTests<Prototype, object> { 
    private readonly TestClass x = new (){ Id = 1, Name = "A", ValidFrom = DateTime.Now,
        Class = new () {Id = 2, Name = "B", ValidFrom = DateTime.Now.AddYears(-100)}, 
        OtherClass = new (){Value = 3} };
    private TestClass? y;
    [TestInitialize] public override void Initialize() {
        base.Initialize();
        y = new Prototype().Clone(x);
    }
    [TestMethod] public void CloneTest()  => notSame(x, y);
    [TestMethod] public void IdTest()  => equal(x.Id, y?.Id);
    [TestMethod] public void NameTest()  => equal(x.Name, y?.Name);
    [TestMethod] public void NameIsDeapCloneTest() {
        if (y is null) Assert.Fail();
        equal(x.Name, y.Name);
        y.Name = "C";
        equal("A", x.Name);
        equal("C", y.Name);
    }
    [TestMethod] public void ValidFromTest()  => equal(x.ValidFrom, y?.ValidFrom);
    [TestMethod] public void ClassTest()  => notSame(x.Class, y?.Class);
    [TestMethod] public void ClassMustBeADeapCloneTest() {
        if (y is null) Assert.Fail();
        if (y.Class is null) Assert.Fail();
        equal(x.Class?.Name, y.Class.Name);
        y.Class.Name = "C";
        equal("B", x.Class?.Name);
        equal("C", y.Class.Name);
    }
    [TestMethod] public void OtherClassTest()  => notSame(x.OtherClass, y?.OtherClass);
    [TestMethod] public void OtherClassMustBeADeapCloneTest() {
        if (y is null) Assert.Fail();
        if (y.OtherClass is null) Assert.Fail();
        equal(x.OtherClass?.Value, y.OtherClass.Value);
        y.OtherClass.Value = 5;
        equal(3, x.OtherClass?.Value);
        equal(5, y.OtherClass.Value);
    }

}
