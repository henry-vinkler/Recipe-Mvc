namespace RecipeMvc.Tests;

public abstract class AbstractTests<TClass, TBaseClass> : BaseClassTests<TClass, TBaseClass>
    where TClass : class
    where TBaseClass : class {
    [TestMethod] public void IsAbstarctTest() => isTrue(typeof(TClass).IsAbstract);
}

