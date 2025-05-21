using System.Reflection;

namespace RecipeMvc.Tests;

public abstract class SealedTests<TClass, TBaseClass> : ClassTests<TClass, TBaseClass>
    where TClass : class, new()
    where TBaseClass : class {
    [TestMethod] public void IsSealedTest() => isTrue(typeof(TClass).IsSealed);
}

public abstract class ClassTests<TClass, TBaseClass> : BaseClassTests<TClass, TBaseClass> 
    where TClass : class, new() 
    where TBaseClass : class { 
    protected override TClass createObj() => new ();
}

public abstract class AbstractTests<TClass, TBaseClass> : BaseClassTests<TClass, TBaseClass>
    where TClass : class
    where TBaseClass : class {
    [TestMethod] public void IsAbstarctTest() => isTrue(typeof(TClass).IsAbstract);
}

public abstract class BaseClassTests<TClass, TBaseClass> : BaseTests 
    where TClass : class 
    where TBaseClass : class {
    protected TClass? obj;
    protected abstract TClass createObj();
    [TestInitialize] public virtual void Initialize() {
        type = typeof(TClass);
        obj = createObj();
    }
    [TestCleanup] public virtual void Cleanup() {
        type = null;
        obj = null;
    }
    [TestMethod] public void CanCreateTest() => notNull(obj);
    [TestMethod] public void IsTypeOfTest() => isType(obj, typeof(TClass));
    [TestMethod] public void IsBaseTypeOfTest() => equal(typeof(TClass).BaseType, typeof(TBaseClass));
    [TestMethod] public void IsTested() {
        var testMethods = GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null)
            .Select(m => m.Name)
            .ToArray();

        var members = typeof(TClass)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .Select(m => m.Name)
            .Where(m => !testMethods.Contains(m + "Test"))
            .ToArray();

        if (members.Length == 0) return;

        var notTested = string.Join(", ", members);
        if (members.Length == 1)
            Assert.Fail($"Test method for <{notTested}> not found.");
        Assert.Fail($"Test methods for <{notTested}> not found.");
    }
    protected override void canGet<T>(PropertyInfo pi, T? expected) where T : default {
        var actual = pi.GetValue(obj);
        equal(actual, expected);
    }
    protected override void canSet<T>(PropertyInfo pi, T? v)
        where T : default => pi.SetValue(obj, v);
}

