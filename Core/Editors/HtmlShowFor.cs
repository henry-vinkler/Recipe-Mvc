using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeMvc.Core.Editors;
public static class HtmlShowFor {
    public static IHtmlContent ShowFor<TModel, TResult>(
        this IHtmlHelper<TModel> h, Expression<Func<TModel, TResult>> e) 
            => h.ForShow(e, h.DisplayFor(e));
}
