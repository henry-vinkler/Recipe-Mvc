using RecipeMvc.Aids.GoF.Behav;

namespace RecipeMvc.Aids;

public static class Random {
    private readonly static System.Random r = new System.Random();
    private readonly static DateTime minDt = System.DateTime.Now.AddYears(-100);
    private readonly static DateTime maxDt = System.DateTime.Now.AddYears(100); 
    public static bool Boolean() => Int32() % 2 == 0;
    public static char Char(char min = (char)ushort.MinValue, char max = (char)ushort.MaxValue)
        => (char)Int32(min, max);
    public static char Char(string? allowedChars = null) => 
        (allowedChars == null) 
            ? Char('!', '}') 
            : allowedChars[Int32(0, allowedChars.Length)]; 
    public static DateTime DateTime(DateTime min = default, DateTime max = default)
        => new (Int64(ticks(min), ticks(max, false)));
    public static decimal Decimal(decimal min = decimal.MinValue, decimal max = decimal.MaxValue) { 
        var i = UInt64();   
        var d = (decimal)i/ulong.MaxValue;
        var dmin = d * min;
        var dmax = d * max; 
        try {
            return min + dmax - dmin;
        } catch {
            return min - dmin + dmax;
        }
    }
    public static double Double(double min = double.MinValue, double max = double.MaxValue) {
        static bool isInfinity(double d) => double.IsInfinity(d) || double.IsNaN(d);
        if (min > max) (min, max) = (max, min);
        var d = r.NextDouble();
        var dmin = d * min;
        var dmax = d * max;
        d = min + dmax - dmin;
        if (isInfinity(d)) d = min - dmin + dmax;
        return d;
    }
    public static object? EnumOf(Type t) {
         if (!t.IsEnum) return null;
         var values = Enum.GetValues(t);
         var index = Int32(0, values.Length);
         return values.GetValue(index);
    }    
    public static float Float(float min = float.MinValue, float max = float.MaxValue)
        => (float)Double(min, max);
    public static sbyte Int8(sbyte min = sbyte.MinValue, sbyte max = sbyte.MaxValue)
        => (sbyte)Int32(min, max);
    public static short Int16(short min = short.MinValue, short max = short.MaxValue)
        => (short)Int32(min, max);
    public static int Int32(int min = int.MinValue, int max = int.MaxValue)
        => (int)Int64(min, max);
    public static long Int64(long min = long.MinValue, long max = long.MaxValue)
        => (min <= max) ? r.NextInt64(min, max) : r.NextInt64(max, min);
    public static string String(ushort minLength = 5, ushort maxLength = 10, 
        string? allowedChars = null) {
        var lenght = UInt16(minLength, maxLength);
        var s = new char[lenght];
        for (var i = 0; i < lenght; i++) s[i] = Char(allowedChars);
        return new (s);
    }
    public static byte UInt8(byte min = byte.MinValue, byte max = byte.MaxValue)
        => (byte)Int32(min, max);
    public static ushort UInt16(ushort min = ushort.MinValue, ushort max = ushort.MaxValue)
        => (ushort)Int32(min, max);
    public static uint UInt32(uint min = uint.MinValue, uint max = uint.MaxValue)
        => (uint)Int64(min, max);
    public static ulong UInt64(ulong min = ulong.MinValue, ulong max = ulong.MaxValue) {
        var min64 = long.MinValue + (long)min;
        var max64 = long.MinValue + (long)max;
        var int64 = Int64(min64, max64);
        var r = (ulong)(int64 - long.MinValue);
        return r;
    }
    public static T Object<T>() where T : class, new() {
        var o = new T();
        return (Object(o) as T)?? o;
    }
    public static object? Object(Type t) {
        var o = Activator.CreateInstance(t);
        return Object(o);
    }
    public static object? Object(object? o) {
        var properties = o?.GetType().GetProperties()??[];
        foreach (var p in properties) {
            if (p.CanWrite) {
                var v = random(p.PropertyType);
                if (v is not null) p.SetValue(o, v);
            }
         }
        return o;
    }
    private static object? random(Type t) {
        if (t is null) return null;
        var s = new Round(2);
        var x = Nullable.GetUnderlyingType(t);
        if (x is not null) t = x;
        if (t.IsEnum) return EnumOf(t);
        if (t == typeof(bool)) return Boolean();
        if (t == typeof(char)) return Char('A', 'Z');
        if (t == typeof(DateTime)) return DateTime(System.DateTime.Now.AddYears(-10), System.DateTime.Now.AddYears(10));
        if (t == typeof(decimal)) return Decimal(-100.00m, 100.00m).DoRound(s);
        if (t == typeof(double)) return Double(-100.0, 100.0).DoRound(s);
        if (t == typeof(float)) return Float(-10.0f, 10.0f).DoRound(s);
        if (t == typeof(sbyte)) return Int8(-10, 10);
        if (t == typeof(short)) return Int16(-100, 100);
        if (t == typeof(int)) return Int32(-1000, 1000);
        if (t == typeof(long)) return Int64(-10000, 10000);
        if (t == typeof(string) ) return String(5, 10, "abcdefghijklmnopqrstuvwxyz");
        if (t == typeof(byte)) return UInt8(0, 10);
        if (t == typeof(ushort)) return UInt16(0, 100);
        if (t == typeof(uint)) return UInt32(0, 1000);
        if (t == typeof(ulong)) return UInt64(0, 10000);
        return null;
    }
    private static long ticks(DateTime dt, bool isMin = true) {
        dt = dt == default ? isMin ? minDt : maxDt : dt;
        return dt.Ticks;
    }
    public static T? Type<T>() => (T?) random(typeof(T));
}