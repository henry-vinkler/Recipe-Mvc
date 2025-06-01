using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeMvc.Aids;
using RecipeMvc.Tests;

namespace RecipeMvc.Tests.Aids;

[TestClass]
public class DaysTests : EnumTests<Days>
{
    public DaysTests() : base(7) { }

    [TestMethod] public void MondayTest() => isEnum((int)Days.Monday);
    [TestMethod] public void TuesdayTest() => isEnum((int)Days.Tuesday);
    [TestMethod] public void WednesdayTest() => isEnum((int)Days.Wednesday);
    [TestMethod] public void ThursdayTest() => isEnum((int)Days.Thursday);
    [TestMethod] public void FridayTest() => isEnum((int)Days.Friday);
    [TestMethod] public void SaturdayTest() => isEnum((int)Days.Saturday);
    [TestMethod] public void SundayTest() => isEnum((int)Days.Sunday);
}
