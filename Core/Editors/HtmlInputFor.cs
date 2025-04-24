using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeMvc.Core.Editors;

public static class HtmlInputFor{
    public static IHtmlContent InputFor<TModel, TResult>( 
        this IHtmlHelper<TModel> h, Expression<Func<TModel, TResult>> e) =>
          h.ForInput(e, h.EditorFor(e, new { htmlAttributes = new { @class = "form-control" } }));
}