using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Text.Encodings.Web;

namespace RecipeMvc.Core.Editors;

public static class HtmlSelectFor
{
    public static IHtmlContent SelectFor<TModel, TResult>(
       this IHtmlHelper<TModel> h, Expression<Func<TModel, TResult>> e,
        SelectList list) =>
       h.ForInput(e, h.DropDownListFor(e, list, new { @class = "form-control" }));


    public static IHtmlContent SelectFor<TModel, TResult>(
       this IHtmlHelper<TModel> h, Expression<Func<TModel, TResult>> e,
        string controller)
    {
        var lab = h.LabelFor(e, new { @class = "control-label" });
        //var displayName = h.DisplayNameFor(e);
        var n = h.NameFor(e);
        var v = h.ValueFor(e);
        var ed = new HtmlString(
            $"<select name=\"{n}\" " +
            $"class=\"selectItems2 form-control\" " +
            $"data-controller=\"{controller}\" " +
            $"data-id=\"{v}\">" +
            $"</select>");
        var val = h.ValidationMessageFor(e, string.Empty, new { @class = "text-danger" });

        var div = new TagBuilder("div");
        div.AddCssClass("form-group");
        div.InnerHtml.AppendHtml(lab);
        div.InnerHtml.AppendHtml(ed);
        div.InnerHtml.AppendHtml(val);

        var writer = new StringWriter();
        div.WriteTo(writer, HtmlEncoder.Default);

        return new HtmlString(writer.ToString());
    }
}
