namespace RecipeMvc.Tests;

public abstract class ClassTests<TClass, TBaseClass> : BaseClassTests<TClass, TBaseClass> 
    where TClass : class, new() 
    where TBaseClass : class { 
    protected override TClass createObj() => new ();
}



