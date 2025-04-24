using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace RecipeMvc.Core.Editors;

public static class HtmlInputForEnum{
    public static IHtmlContent InputForEnum<TModel, TResult>( 
        this IHtmlHelper<TModel> h, Expression<Func<TModel, TResult>> e) => 
        h.ForInput(e, h.DropDownListFor(e, selectList<TResult>(), new { @class = "form-control" }));

    private static SelectList selectList<TEnum>() {
        var t = typeof(TEnum);
        var x = Nullable.GetUnderlyingType(t);
        if (x != null) t = x;
        return new (Enum.GetNames(t));
    }
}
