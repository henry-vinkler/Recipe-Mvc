namespace MvcTunnikontroll.Tests.Aids;

using MvcTunnikontroll.Tests;
using System.Globalization;
using Random = MvcTunnikontroll.Aids.Random;

[TestClass] public sealed class RandomTests : BaseTests {
    private sealed class TestClass {
        public bool? Boolean { get; set; }
        public char? Char { get; set; }
        public DateTime? DateTime { get; set; }
        public decimal? Decimal { get; set; }
        public double? Double { get; set; }
        public float? Float { get; set; }
        public sbyte? Int8 { get; set; }
        public short? Int16 { get; set; }
        public int? Int32 { get; set; }
        public long? Int64 { get; set; }
        public string? String { get; set; }
        public byte? UInt8 { get; set; }
        public ushort? UInt16 { get; set; }
        public uint? UInt32 { get; set; }
        public ulong? UInt64 { get; set; }
    }
    [TestMethod] public void ObjectTest() {
        var o = Random.Object<TestClass>();
        notNull(o);
        notNull(o.Boolean);
        notNull(o.Char);
        notNull(o.DateTime);
        notNull(o.Decimal);
        notNull(o.Double);
        notNull(o.Float);
        notNull(o.Int8);
        notNull(o.Int16);
        notNull(o.Int32);
        notNull(o.Int64);
        notNull(o.String);
        notNull(o.UInt8);
        notNull(o.UInt16);
        notNull(o.UInt32);
        notNull(o.UInt64);
    }

    [TestMethod] public void BooleanTest() => shortTestCycle(Random.Boolean);
    [TestMethod] public void CharTest() => shortTestCycle(() => Random.Char((char)0));
    [TestMethod] public void Int8Test() => shortTestCycle(() => Random.Int8());
    [TestMethod] public void Int16Test() => longTestCycle(() => Random.Int16());
    [TestMethod] public void Int32Test() => longTestCycle(() => Random.Int32());
    [TestMethod] public void Int64Test() => longTestCycle(() => Random.Int64());
    [TestMethod] public void UInt8Test() => shortTestCycle(() => Random.UInt8());
    [TestMethod] public void UInt16Test() => longTestCycle(() => Random.UInt16());
    [TestMethod] public void UInt32Test() => longTestCycle(() => Random.UInt32());
    [TestMethod] public void UInt64Test() => longTestCycle(() => Random.UInt64());

    [DataRow(0UL, 1UL)]
    [DataRow(1UL, 2UL)]
    [DataRow(2UL, 3UL)]
    [DataRow(0UL, 100UL)]
    [DataRow(ulong.MaxValue - 100, ulong.MaxValue)]
    [DataRow(ulong.MaxValue - 3, ulong.MaxValue - 2)]
    [DataRow(ulong.MaxValue - 2, ulong.MaxValue - 1)]
    [DataRow(ulong.MaxValue - 1, ulong.MaxValue)]
    [TestMethod] public void UInt64Test(ulong min, ulong max) {
        var i = Random.UInt64(min, max);
        isTrue( i >= min && i < max, i.ToString());
    }
 
    [TestMethod] public void StringTest() => longTestCycle(() => Random.String());
    [TestMethod] public void DateTimeTest() => longTestCycle(() => Random.DateTime());
    [TestMethod] public void DecimalTest() => longTestCycle(() => Random.Decimal());

    [DataRow("-79228162514264337593543950335", "-79228162514264337593500000000")]
    [DataRow("-100", "0")]
    [DataRow("-1", "0")]
    [DataRow("-1", "1")]
    [DataRow("-100", "100")]
    [DataRow("-1000000", "1000000")]
    [DataRow("0", "1")]
    [DataRow("0", "100")]
    [DataRow( "79228162514264337593500000000",  "79228162514264337593543950335")]
    [DataRow("-79228162514264337593543950335", "79228162514264337593543950335")]
    [TestMethod] public void DecimalTest(string min, string max) {
        var minD = decimal.Parse(min, CultureInfo.InvariantCulture);
        var maxD = decimal.Parse(max, CultureInfo.InvariantCulture);
        for (var i = 0; i < repeatCount; i++) {
            var d = Random.Decimal(minD, maxD);
            isTrue(d >= minD && d < maxD, d.ToString());
        }
    }
    [TestMethod] public void FloatTest()=> longTestCycle(() => Random.Float());
    [TestMethod] public void DoubleTest() => longTestCycle(() => Random.Double());

    [DataRow(double.MinValue, -1.797693134E+308)]
    [DataRow(double.MinValue, -1d)]
    [DataRow(double.MinValue, -100d)]
    [DataRow(double.MinValue, 0d)]
    [DataRow(double.MinValue, 1d)]
    [DataRow(double.MinValue, 100d)]
    [DataRow(-1d, 0d)]
    [DataRow(-1d, 1d)]
    [DataRow(-1E-310, 1E-310)] //double.Epsylon = 4.94065645841247E-324
    [DataRow(0d, 1d)]
    [DataRow(-100d, double.MaxValue)]
    [DataRow(-1d, double.MaxValue)]
    [DataRow(0d, double.MaxValue)]
    [DataRow(1d, double.MaxValue)]
    [DataRow(100d, double.MaxValue)]
    [DataRow(1.797693134E+308, double.MaxValue)]
    [DataRow(double.MinValue, double.MaxValue)]
    [TestMethod] public void DoubleTest(double min, double max) {
        for (var i = 0; i < repeatCount; i++) {
            var d = Random.Double(min, max);
            isTrue(d >= min && d < max, d.ToString());
        }
    }

    [DataRow(long.MinValue,     9223372036854775808UL)] //10000000000000000...0000
    [DataRow(long.MinValue + 1, 9223372036854775809UL)] //10000000000000000...0001
    [DataRow(long.MinValue + 2, 9223372036854775810UL)]
    [DataRow(-3,               18446744073709551613UL)]
    [DataRow(-2,               18446744073709551614UL)]
    [DataRow(-1,                       ulong.MaxValue)] // 1111111111111111...1111
    [DataRow(0,                        ulong.MinValue)] // 0000000000000000...0000
    [DataRow(1,                                   1UL)] // 0000000000000000...0001
    [DataRow(2,                                   2UL)] // 0000000000000000...0010
    [DataRow(3,                                   3UL)] // 0000000000000000...0011
    [DataRow(long.MaxValue - 2, 9223372036854775805UL)] // 0111111111111111...1101
    [DataRow(long.MaxValue - 1, 9223372036854775806UL)] // 0111111111111111...1110
    [DataRow(long.MaxValue,     9223372036854775807UL)] // 0111111111111111...1111
    [TestMethod] public void Int64ToUInt64Test(long l, ulong u) => equal((ulong)l, u);

    [DataRow("0001-01-01T00:00:00.0000000",                   0L)] // 0000000000000000...0000
    [DataRow("0001-01-01T00:00:00.0000001",                   1L)] // 0000000000000000...0001
    [DataRow("0001-01-01T00:00:01.0000000",            10000000L)] // sekund
    [DataRow("0001-01-01T00:01:00.0000000",           600000000L)] // minut
    [DataRow("0001-01-01T01:00:00.0000000",         36000000000L)] // tund
    [DataRow("0001-01-02T00:00:00.0000000",        864000000000L)] // päev
    [DataRow("0001-02-01T00:00:00.0000000",      26784000000000L)] // 01.02.0001
    [DataRow("0002-01-01T00:00:00.0000000",     315360000000000L)] // 01.01.0002
    [DataRow("9999-12-31T23:59:59.9999999", 3155378975999999999L)] // maksimaalne kuupäev
    [TestMethod] public void DateTimeToInt64Test(string s, long l) {
        var d = DateTime.Parse(s);
        equal(d.Ticks, l);
    }

    private void shortTestCycle<T>(Func<T> methodToTest) where T : IComparable<T> {
        var a = methodToTest();
        T b;
        do {
            b = methodToTest();
        } while (b.CompareTo(a) != 0);
    }
    private void longTestCycle<T>(Func<T> methodToTest) {
        var a = methodToTest();
        var i = 0;
        do {
            var b = methodToTest();
            if (a?.Equals(b) ?? false) continue;
            notEqual(a, b);
            i++;
        } while (i < repeatCount);
        equal(i, repeatCount);
    }
}