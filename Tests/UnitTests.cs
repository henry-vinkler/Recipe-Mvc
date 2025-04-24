namespace MvcTunnikontroll.Tests;

[TestClass] public sealed class UnitTests: BaseTests {
    private object? x;
    private object? y;
    [TestInitialize] public void Initialize() {
        x = new object();
        y = new object();
    }
    [TestCleanup] public void Cleanup() {
        x = null;
        y = null;
    }
    [DataRow("abc", "abc")]
    [DataRow(1, 1)]
    [DataRow(1.23, 1.23)]
    [DataRow(true, true)]
    [DataRow(false, false)]
    [TestMethod] public void AreEqualTest(object x, object y) => equal(x, y);
    //[TestMethod] public void AreEqualFailsTest() => Assert.AreEqual(1, 2);
    [TestMethod] public void AreNotEqualTest() => notEqual("aaa", "bbb");
    //[TestMethod] public void AreNotEqualFailsTest() => Assert.AreNotEqual("abc", "abc");
    [TestMethod] public void IsTrueTest() => isTrue(true);
    //[TestMethod] public void IsTrueFailsTest() => Assert.IsTrue(false);
    [TestMethod] public void IsFalseTest() => isFalse(false);
    //[TestMethod] public void IsFalseFailsTest() => Assert.IsFalse(true);
    [TestMethod] public void IsNullTest() => isNull<object>(null);
    //[TestMethod] public void IsNullFailsTest() => Assert.IsNull("abc");
    [TestMethod] public void IsNotNullTest() => notNull("abc");
    //[TestMethod] public void IsNotNullFailsTest() => Assert.IsNotNull(null);
    [TestMethod] public void IsInstanceOfTypeTest() => isType("abc", typeof(string));
    //[TestMethod] public void IsInstanceOfTypeFailsTest() => Assert.IsInstanceOfType("abc", typeof(int));
    [TestMethod] public void IsNotInstanceOfTypeTest() => notType("abc", typeof(int));
    //[TestMethod] public void IsNotInstanceOfTypeFailsTest() => Assert.IsNotInstanceOfType("abc", typeof(string));
    //[TestMethod] public void FailTest() => Assert.Fail();
    //[TestMethod] public void FailFailsTest() => Assert.Fail();
    [TestMethod] public void InconclusiveTest() => notTested("See test vajab tegemist");
    [TestMethod] public void AreSameTest() {
        var y = x;
        same (x, y);
    }
    //[TestMethod] public void AreSameFailsTest() => Assert.AreSame (x, y);
    [TestMethod] public void AreNotSameTest()  => notSame (x, y);
    //[TestMethod] public void AreNotSameFailsTest() {
    //    var y = x;
    //    Assert.AreNotSame (x, y);
    //}
}
