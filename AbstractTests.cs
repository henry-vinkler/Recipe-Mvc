namespace Mvc.Tests;

public abstract class AbstractTests<TClass, TBaseClass> : BaseClassTests<TClass, TBaseClass> 
    where TClass : class 
    where TBaseClass : class { 
    [TestMethod] public void IsAbstractTest() => isTrue(typeof(TClass).IsAbstract);    
}
