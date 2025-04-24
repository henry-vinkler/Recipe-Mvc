using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeMvc.Core.Editors;
public static class HtmlForShow {
    public static IHtmlContent ForShow<TModel, TResult>(
        this IHtmlHelper<TModel> h, Expression<Func<TModel, TResult>> e, params IHtmlContent[] controls) {
        var l = h.DisplayNameFor(e);
        var dt = new TagBuilder("dt");
        dt.AddCssClass("col-sm-2");
        dt.InnerHtml.Append(l);

        var dd = new TagBuilder("dd");
        dd.AddCssClass("col-sm-10");
        foreach (var control in controls) 
            dd.InnerHtml.AppendHtml(control);
        
        var c = new HtmlContentBuilder();
        c.AppendHtml(dt);
        c.AppendHtml(dd);

        return c;
    }
}
