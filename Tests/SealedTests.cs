using System.ComponentModel;
using System.Reflection;
using RecipeMvc.Tests;

namespace Mvc.Tests;

public abstract class SealedTests<TClass, TBaseClass> : ClassTests<TClass, TBaseClass>
    where TClass : class, new()
    where TBaseClass : class {
    [TestMethod] public void IsSealedTest() => isTrue(typeof(TClass).IsSealed);
    [TestMethod] public virtual void DisplayNameTest() => isDisplayName();
    protected void isDisplayName(string? displayName = null) {
        var actual = type?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
        equal(displayName, actual);
    }
}
