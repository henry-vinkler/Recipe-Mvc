using System.Reflection;

namespace RecipeMvc.Tests;

public abstract class BaseClassTests<TClass, TBaseClass> : StaticTests 
    where TClass : class 
    where TBaseClass : class {
    protected TClass? obj;
    protected abstract TClass createObj();
    protected override Type setType() => typeof(TClass);
    [TestInitialize] public override void Initialize() {
        base.Initialize();
        this.obj = createObj();
    }
    [TestCleanup] public override void Cleanup() {
        base.Cleanup();
        obj = null;
   }
    [TestMethod] public void CanCreateTest() => notNull(obj);
    [TestMethod] public void IsTypeOfTest() => isType(obj, typeof(TClass));
    [TestMethod] public virtual void IsBaseTypeOfTest() => equal(typeof(TClass).BaseType, typeof(TBaseClass));
    protected override void canGet<T>(PropertyInfo pi, T? expected) where T: default  {
        var actual = pi.GetValue(obj);
        equal(actual, expected);
    }
    protected override void canSet<T>(PropertyInfo pi, T? v) where T: default 
        => pi.SetValue(obj, v);
}
