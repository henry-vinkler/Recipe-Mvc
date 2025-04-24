using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeMvc.Core.Editors;
public static class HtmlShowTable{
    public static IHtmlContent ShowTable<TModel>( 
        this IHtmlHelper<IEnumerable<TModel>> h, IEnumerable<TModel> list) {
            var properties = getProperties<TModel>();
            var tbl = createTable();
            tbl.InnerHtml.AppendHtml( h.createHeader(properties) );
            tbl.InnerHtml.AppendHtml( h.createBody(properties, list) );
            return tbl;
        }
    private static PropertyInfo[] getProperties<TModel>() 
        => typeof(TModel)?.GetProperties().Where(p => p.Name != "Id").ToArray()??[];
    private static TagBuilder createTable() {
        var t = tblTag;
        t.AddCssClass("table");
        return t;
    }
     private static TagBuilder tblTag => new ("table");
    private static IHtmlContent createHeader<TModel> (
        this IHtmlHelper<IEnumerable<TModel>> h, PropertyInfo[] properties) {
        var header = tblHdrTag;
        var row = tblRowTag;
        foreach (var p in properties) {
            var col = tblColTag;
            var name = displayNameFor(p);
            name = h.updateDisplayName(name, p.Name);
            var sortOrder = h.updateSortOrder(p.Name);
            var link = $"<a href=\"?pageIdx=0&orderBy={sortOrder}&filter={h.ViewBag.Filter}\">{name}</a>";
            col.InnerHtml.AppendHtml(link);
            row.InnerHtml.AppendHtml(col);
        }
        row.InnerHtml.AppendHtml(tblColTag);
        header.InnerHtml.AppendHtml(row);
        return header;
    }
    private static string updateDisplayName<TModel> (
        this IHtmlHelper<IEnumerable<TModel>> h, string name, string propName) {
        var sortOrder = h.ViewBag.OrderBy as string;
        if (sortOrder == propName) name += " ▲";
        else if (sortOrder == propName + "_desc") name += " ▼";
        return name;
    }
    private static string updateSortOrder<TModel> (
        this IHtmlHelper<IEnumerable<TModel>> h, string propName) {
        var sortOrder = h.ViewBag.OrderBy as string;
        if (sortOrder == propName) return propName += "_desc";
        else return propName;
    }
    private static TagBuilder tblHdrTag => new ("thead");
    private static TagBuilder tblRowTag => new ("tr");
    private static TagBuilder tblColTag => new ("td");
    private static string displayNameFor(PropertyInfo p) 
        => p.GetCustomAttribute<DisplayAttribute>()?.Name?? p.Name;
    private static IHtmlContent createBody<TModel>(this IHtmlHelper<IEnumerable<TModel>> h,
        PropertyInfo[] properties, IEnumerable<TModel> list) {
        var tblBody = tblBodyTag;
        foreach (var item in list) {
            var row = tblRowTag;
            foreach (var p in properties) {
                var data = tblDataTag;
                var value = getValue(p, item);
                data.InnerHtml.AppendHtml(value);
                row.InnerHtml.AppendHtml(data);
            }
            h.addHrefs(row, item);
            tblBody.InnerHtml.AppendHtml(row);
        }
        return tblBody;
    }
    private static TagBuilder tblBodyTag => new("tbody");
    private static TagBuilder tblDataTag => new("td");
    private static IHtmlContent getValue<TModel>(PropertyInfo? p, TModel? item) {
        var v = p?.GetValue(item);
        var dt = v as DateTime?;
        if (dt != null) return new HtmlString(dt?.ToShortDateString() ?? "");
        return new HtmlString(v?.ToString() ?? "");
    }
    private static void addHrefs<TModel>(
        this IHtmlHelper<IEnumerable<TModel>> h, TagBuilder row, TModel item) {
        var id = getId<TModel>(item);
        var td = tblDataTag;
        var ed = h.hrefEd(id);
        var det = h.hrefDet(id);
        var del = h.hrefDel(id);
        td.InnerHtml.AppendHtml(ed);
        td.InnerHtml.Append(" ");
        td.InnerHtml.AppendHtml(det);
        td.InnerHtml.Append(" ");
        td.InnerHtml.AppendHtml(del);
        row.InnerHtml.AppendHtml(td);
    }
    private static int getId<TModel>(object? item)
        => typeof(TModel).GetProperty("Id")?.GetValue(item) as int? ?? 0;
    private static IHtmlContent hrefEd<TModel>(this IHtmlHelper<IEnumerable<TModel>> h, int i)
        => h.ActionLink("Edit", "Edit", new { id = i });
    private static IHtmlContent hrefDet<TModel>(this IHtmlHelper<IEnumerable<TModel>> h, int i)
        => h.ActionLink("Details", "Details", new { id = i });
    private static IHtmlContent hrefDel<TModel>(this IHtmlHelper<IEnumerable<TModel>> h, int i)
        => h.ActionLink("Delete", "Delete", new { id = i });
}