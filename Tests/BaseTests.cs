namespace RecipeMvc.Tests;
public abstract class BaseTests {
    protected const int repeatCount = 1000;
    protected void equal<T>(T? x, T? y, string? msg = null) => Assert.AreEqual(x, y, msg);
    protected void notEqual<T>(T? x, T? y, string? msg = null) => Assert.AreNotEqual(x, y, msg);
    protected void isTrue(bool x, string? msg = null) => Assert.IsTrue(x, msg);
    protected void isFalse(bool x, string? msg = null) => Assert.IsFalse(x, msg);
    protected void isNull<T>(T? x, string? msg = null) => Assert.IsNull(x, msg);
    protected void notNull<T>(T? x, string? msg = null) => Assert.IsNotNull(x, msg);
    protected void isType(object? x, Type t, string? msg = null) => Assert.IsInstanceOfType(x, t, msg);
    protected void notType(object? x, Type t, string? msg = null) => Assert.IsNotInstanceOfType(x, t, msg);
    protected void notTested(string? msg = null) => Assert.Inconclusive(msg);
    protected void same<T>(T? x, T? y, string? msg = null) => Assert.AreSame(x, y, msg);
    protected void notSame<T>(T? x, T? y, string? msg = null) => Assert.AreNotSame(x, y, msg);
    protected void fail(string? msg = null) => Assert.Fail(msg);
}

