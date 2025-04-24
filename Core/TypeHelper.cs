using System.ComponentModel;

namespace RecipeMvc.Core;

public static class TypeHelper{
    public static string DisplayName<T>(IEnumerable<T> _, string? defaultName= null) 
        => DisplayName(typeof(T), defaultName);
    public static string DisplayName(object? o, string? defaultName= null) 
        => DisplayName(o?.GetType(), defaultName);
    public static string DisplayName(Type? t, string? defaultName= null) {
        DisplayNameAttribute? a = null;
        if (t is not null)
            a = Attribute.GetCustomAttribute(t, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
        return a?.DisplayName ?? t?.Name ?? defaultName ?? string.Empty;    
    }
}
