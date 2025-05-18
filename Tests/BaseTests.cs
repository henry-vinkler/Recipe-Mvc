using System.Diagnostics;
using System.Reflection;
using RecipeMvc.Aids;
namespace RecipeMvc.Tests;
public abstract class BaseTests {
    protected const int repeatCount = 1000;
    protected Type? type;
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
    protected void isProperty<T>(bool isReadOnly = false) {
        var pi = getPropertyInfo();
        notNull(pi);
        isType<T>(pi!);
        canRead(pi!);
        if (!isReadOnly) canWrite(pi!);
        var v = RecipeMvc.Aids.Random.Type<T>();
        canSet(pi!, v);
        canGet(pi!, v);
    }
    protected PropertyInfo? getPropertyInfo() {
        var n = getPropertyName();
        if (n is null) return null;
        return type?.GetProperty(n);
    }
    protected static string? getPropertyName() {
        var stack = new StackTrace();
        for (var i = 1; i < stack.FrameCount; i++) {
            var m = stack.GetFrame(i)?.GetMethod();
            if (m is null) continue;
            var isTest = m.GetCustomAttributes(typeof(TestMethodAttribute), true).Any();
            if (isTest) return m.Name.Replace("Test", string.Empty);
        }
        return null;
    }
    private void isType<T>(PropertyInfo pi) {
        if (pi!.PropertyType?.Name != typeof(T).Name)
            fail($"Property {pi.Name} is not of type {typeof(T).Name}");
    }
    protected void canRead(PropertyInfo pi) =>
        isTrue(pi.CanRead, $"Property {pi.Name} is not readable");
    protected void canWrite(PropertyInfo pi) =>
        isTrue(pi.CanWrite, $"Property {pi.Name} is not writable");
    protected virtual void canSet<T>(PropertyInfo pi, T? v) => fail();
    protected virtual void canGet<T>(PropertyInfo pi, T? v) => fail();
}

