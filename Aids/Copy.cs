namespace RecipeMvc.Aids;

public static class Copy { 
    public static TTo Members<TFrom, TTo>(TFrom from, TTo to) where TFrom: class where TTo: class {
        var properties = from.GetType().GetProperties();
        foreach (var p in properties) {
            var n = p.Name;
            var v = p.GetValue(from);
            var t = to.GetType().GetProperty(n);
            if (v is null) continue;
            if (t is null) continue;
            var pt = t.PropertyType;
            pt = Nullable.GetUnderlyingType(pt) ?? pt;
            if (v.GetType() != pt) continue;
            if (!t.CanWrite) continue;
            t.SetValue(to, v);
        }
        return to;
    } 
}