namespace RecipeMvc.Tests;

public abstract class SealedTests<TClass, TBaseClass> : ClassTests<TClass, TBaseClass>
    where TClass : class, new()
    where TBaseClass : class {
    [TestMethod] public void IsSealedTest() => isTrue(typeof(TClass).IsSealed);
}

