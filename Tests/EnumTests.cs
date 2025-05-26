using System.Reflection;

namespace RecipeMvc.Tests;

public abstract class EnumTests<TEnum>(int count)  : StaticTests where TEnum: Enum {
    protected override Type? setType() => typeof (TEnum);
    protected string[] memberNames {get; set;} = getNames();
    private static string[] getNames() {
       var t = typeof(TEnum);
       var fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);
       var names = fields.Select(f => f.Name);
       var result = names.ToArray();
       return result;    
    }
    [TestMethod] public void CountTest() => equal(memberNames.Length, count);
    [TestMethod] public override void IsTested() {
        var testMethods = GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null)
            .Select(m => m.Name).ToArray();
        notNull (type);
        var members = memberNames
            .Where(name => !testMethods.Contains(name + "Test"))
            .ToArray();
        if (members.Length == 0) return;
        var notTestedMembers = string.Join(", ", members);
        if (members.Length == 1) 
            notTested($"Test method for <{notTestedMembers}> not found.");
        notTested($"Test methods for <{notTestedMembers}> not found.");
    } 
    protected void isEnum(int value) {
        var name = getPropertyName();
        notNull(name);
        var actual = Enum.Parse(typeof(TEnum), name!);
        equal((int) actual, value);
    }
}
