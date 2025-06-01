using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RecipeMvc.Data;
using RecipeMvc.Soft.Controllers;
using RecipeMvc.Soft.Data;
using RecipeMvc.Tests;

namespace RecipeMvc.Tests.Soft.Views;

public abstract class HostTests<TController, TData> : BaseTests
    where TController : Controller where TData : EntityData<TData>, new() {
    private static TestHost<Program, ApplicationDbContext>? host;
    private static HttpClient? client;
    private DbContext? dbContext => host?.db;
    private DbSet<TData>? dbSet => dbContext?.Set<TData>();
    private string BaseUrl => typeof(TController).Name.Replace("Controller", string.Empty);
    static HostTests() {
         host = new TestHost<Program, ApplicationDbContext>();
         client = host.startClient();
         host.startDb();
    }
    public HostTests() => host?.seedData<TData>();
    private async Task<HttpResponseMessage> ensureSuccess(string url) {
         var r = await client!.GetAsync(url);
         r.EnsureSuccessStatusCode();
         return r;
     }
    private async Task<string> getHtmlString(string url) {
         var r = await ensureSuccess(url);
         return await r.Content.ReadAsStringAsync();
    }
    [TestMethod] public void DbSetHasItemsTest() => isTrue(dbSet?.Any() ?? false);
    // [TestMethod] public async Task IndexTest() {
    //     var html = await getHtmlString(BaseUrl);
    //     isTrue(html.Contains("<table") || html.Contains("class=\"list\""));
    // }
    [TestMethod] public async Task CreateTest() {
         var html = await getHtmlString(BaseUrl + "/Create");
         isTrue(html.Contains("<h1>Create</h1>"));
    }
     [TestMethod] public async Task EditTest(){
          var d = host!.createData<TData>();
          host.addToSet(d);
          var html = await getHtmlString(BaseUrl + "/Edit" + $"/{d.Id}");
          isTrue(html.Contains("<h1>Edit</h1>"));
          validate(html, d);
    }
    [TestMethod] public async Task DetailsTest() {
         var d = host!.createData<TData>();
         host.addToSet(d);
         var html = await getHtmlString(BaseUrl + "/Details" + $"/{d.Id}");
         isTrue(html.Contains("<h1>Details</h1>"));
         validate(html, d);
    }
    [TestMethod] public async Task DeleteTest() {
         var d = host!.createData<TData>();
         host.addToSet(d);
         var html = await getHtmlString(BaseUrl + "/Delete"  + $"/{d.Id}");
         isTrue(html.Contains("<h1>Delete</h1>"));
         validate(html, d);
    }
      private void validate(string html, TData d) {
          int found = 0;
          var r = string.Empty;
          var props = typeof(TData).GetProperties();
          foreach (var pi in props) {
               var v = pi.GetValue(d);
               if (v is null) continue;
               var s = (v is DateTime x)? x.ToShortDateString() : v.ToString();
               if (s is null) continue;
               if (html.Contains($">{s}<") || html.Contains($"value=\"{s}\"") ) found++;
               else r = string.IsNullOrWhiteSpace(r) ? s : r + $", {s}"; 
          }
          if (found > 0) return;
          equal(r, html);
    }

   
}
