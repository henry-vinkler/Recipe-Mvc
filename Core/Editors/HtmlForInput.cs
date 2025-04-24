using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeMvc.Core.Editors;

public static class HtmlForInput {
    public static IHtmlContent ForInput<TModel, TResult>(
        this IHtmlHelper<TModel> h, Expression<Func<TModel, TResult>> e, IHtmlContent editor) 
            => h.ForShow(e, editor, 
                h.ValidationMessageFor(e, "", new { @class = "text-danger" }));
}
