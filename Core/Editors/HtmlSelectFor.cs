using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeMvc.Core.Editors;
namespace Mvc.Core.Editors;

public static class HtmlSelectFor
{
    public static IHtmlContent SelectFor<TModel, TResult>(
        this IHtmlHelper<TModel> h, Expression<Func<TModel, TResult>> e, SelectList list) =>
        h.ForInput(e, h.DropDownListFor(e, list, new { @class = "form-control" }));
}
