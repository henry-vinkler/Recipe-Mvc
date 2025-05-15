using Microsoft.Extensions.DependencyInjection;

namespace RecipeMvc.Core;
public static class Services
{
    private static IServiceProvider? sp;
    public static void init(IServiceCollection c) => sp = c?.BuildServiceProvider();
    public static T? Get<T>() => Get(typeof(T));
    public static dynamic? Get(Type t) => sp?.GetRequiredService(t);
}
