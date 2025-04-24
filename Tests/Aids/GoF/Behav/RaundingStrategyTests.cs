namespace RecipeMvc.Tests.Aids.GoF.Behav;
using RecipeMvc.Aids.GoF.Behav;
[TestClass] public class RoundingStrategyTests: BaseTests {
    const double x = 14.451;
    const double y = -14.451;
    [DataRow(3, 5, x, x)]
    [DataRow(3, 5, y, y)]
    [DataRow(2, 5, x, 14.45)]
    [DataRow(2, 5, y, -14.45)]
    [DataRow(1, 5, x, 14.5)]
    [DataRow(1, 5, y, -14.5)]
    [DataRow(0, 5, x, 14)]
    [DataRow(0, 5, y, -14)]
    [DataRow(-1, 5, x, 10)]
    [DataRow(-1, 5, y, -10)]
    [DataRow(3, 4, x, x)]
    [DataRow(3, 4, y, y)]
    [DataRow(2, 4, x, 14.45)]
    [DataRow(2, 4, y, -14.45)]
    [DataRow(1, 4, x, 14.5)]
    [DataRow(1, 4, y, -14.5)]
    [DataRow(0, 4, x, 15)]
    [DataRow(0, 4, y, -15)]
    [DataRow(-1, 4, x, 20)]
    [DataRow(-1, 4, y, -20)]
    [TestMethod] public void RoundTest(int decimals, int roundingDigit, 
           double value, double expected) {
        var s = new Round(decimals, roundingDigit);
        equal(expected, value.DoRound(s));
    }
    [DataRow(3, x, x)]
    [DataRow(3, y, y)]
    [DataRow(2, x, 14.46)]
    [DataRow(2, y, -14.46)]
    [DataRow(1, x, 14.5)]
    [DataRow(1, y, -14.5)]
    [DataRow(0, x, 15)]
    [DataRow(0, y, -15)]
    [DataRow(-1, x, 20)]
    [DataRow(-1, y, -20)]
    [TestMethod] public void RoundUpTest(int decimals, double value, double expected) {
        var s = new RoundUp(decimals);
        equal(expected, value.DoRound(s));
    }
    [DataRow(3, x, x)]
    [DataRow(3, y, y)]
    [DataRow(2, x, 14.45)]
    [DataRow(2, y, -14.45)]
    [DataRow(1, x, 14.4)]
    [DataRow(1, y, -14.4)]
    [DataRow(0, x, 14)]
    [DataRow(0, y, -14)]
    [DataRow(-1, x, 10)]
    [DataRow(-1, y, -10)]
    [TestMethod] public void RoundDownTest(int decimals, double value, double expected) {
        var s = new RoundDown(decimals);
        equal(expected, value.DoRound(s));
    }
    [DataRow(10, x, 20)]
    [DataRow(10, y, -20)]
    [DataRow(5, x, 15)]
    [DataRow(5, y, -15)]
    [DataRow(2, x, 16)]
    [DataRow(2, y, -16)]
    [DataRow(1, x, 15)]
    [DataRow(1, y, -15)]
    [DataRow(0.5, x, 14.5)]
    [DataRow(0.5, y, -14.5)]
    [DataRow(0.25, x, 14.5)]
    [DataRow(0.25, y, -14.5)]
    [DataRow(0.2, x, 14.6)]
    [DataRow(0.2, y, -14.6)]
    [DataRow(0.1, x, 14.5)]
    [DataRow(0.1, y, -14.5)]
    [DataRow(0, x, x)]
    [DataRow(0, y, y)]
    [DataRow(-1, x, x)]
    [DataRow(-1, y, y)]
    [TestMethod] public void RoundUpByStepTest(double step, double value, double expected) {
        var s = new RoundUpByStep(step);
        equal(expected, value.DoRound(s));
    }

    [DataRow(10, x, 10)]
    [DataRow(10, y, -10)]
    [DataRow(5, x, 10)]
    [DataRow(5, y, -10)]
    [DataRow(2, x, 14)]
    [DataRow(2, y, -14)]
    [DataRow(1, x, 14)]
    [DataRow(1, y, -14)]
    [DataRow(0.5, x, 14)]
    [DataRow(0.5, y, -14)]
    [DataRow(0.25, x, 14.25)]
    [DataRow(0.25, y, -14.25)]
    [DataRow(0.2, x, 14.40)]
    [DataRow(0.2, y, -14.40)]
    [DataRow(0.1, x, 14.4)]
    [DataRow(0.1, y, -14.4)]
    [DataRow(0, x, x)]
    [DataRow(0, y, y)]
    [DataRow(-1, x, x)]
    [DataRow(-1, y, y)]
    [TestMethod] public void RoundDownByStepTest(double step, double value, double expected) {
        var s = new RoundDownByStep(step);
        equal(expected, value.DoRound(s));
    }

    [DataRow(3, x, x)]
    [DataRow(2, x, 14.46)]
    [DataRow(1, x, 14.5)]
    [DataRow(0, x, 15)]
    [DataRow(-1, x, 20)]
    [DataRow(3, y, y)]
    [DataRow(2, y, -14.45)]
    [DataRow(1, y, -14.4)]
    [DataRow(0, y, -14)]
    [DataRow(-1, y, -10)]
    [TestMethod] public void RoundTowardsPositiveTest(int decimals, double value, double expected) {
        var s = new RoundTowardsPositive(decimals);
        equal(expected, value.DoRound(s));
    }
    [DataRow(3, y, y)]
    [DataRow(2, y, -14.46)]
    [DataRow(1, y, -14.5)]
    [DataRow(0, y, -15)]
    [DataRow(-1, y, -20)]
    [DataRow(3, x, x)]
    [DataRow(2, x, 14.45)]
    [DataRow(1, x, 14.4)]
    [DataRow(0, x, 14)]
    [DataRow(-1, x, 10)]
    [TestMethod] public void RoundTowardsNegativeTest(int decimals, double value, double expected) {
         var s = new RoundTowardsNegative(decimals);
        equal(expected, value.DoRound(s));
    }
}

