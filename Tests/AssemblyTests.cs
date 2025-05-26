using System.Reflection;

namespace RecipeMvc.Tests;

public abstract class AssemblyTests(string namespaceName): StaticTests {
   protected override Type? setType() => null;
   [TestMethod] public override void IsTested() {
        var testNamespace = string.Empty;
        if (namespaceName.StartsWith("Mvc.")) testNamespace = namespaceName.Replace("Mvc", "Mvc.Tests");
        else testNamespace = "Mvc.Tests." + namespaceName;
        var testAssembly = Assembly.GetExecutingAssembly();
        var testAssemblyTypes = testAssembly.GetTypes();
        var testClasses = testAssemblyTypes
            .Where(t => (t?.Namespace is not null) && t.Namespace.StartsWith(testNamespace))
            .Select(t => t.Name)
            .ToArray();
        var domain = AppDomain.CurrentDomain;
        var assemblies = domain.GetAssemblies();
        var assemblyName = namespaceName.Replace("Mvc", "");
        var assembly = assemblies.FirstOrDefault(a => a.FullName?.StartsWith(namespaceName)?? false);
        if (assembly == null) assembly = assemblies.FirstOrDefault(a => a.FullName?.StartsWith(assemblyName)?? false);
        if (assembly == null) notTested($"Assembly {namespaceName} not found.");
        var classes = assembly?.GetTypes()
            .Where(t => !t.IsInterface && t.IsPublic)
            .Select(t => t.Name)
            .Select(t => {
                var i = t.IndexOf('`');
                return i > 0 ? t.Substring(0, i) : t;
            })
            .Distinct()
            .Where(t => !testClasses.Contains(t+"Tests")).ToArray();
        if (classes?.Length == 0) return;
        var notTestedClasses = string.Join(", ", classes??[]);
        if (classes?.Length == 1) 
            notTested($"Test class for <{notTestedClasses}> not found.");
        notTested($"Test classes for <{notTestedClasses}> not found.");
    }
}
