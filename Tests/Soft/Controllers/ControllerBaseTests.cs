using Microsoft.AspNetCore.Mvc;
using Mvc.Data;
using Mvc.Domain;
using Mvc.Facade;
using Mvc.Soft.Controllers;
using Random = Mvc.Aids.Random;

namespace Mvc.Tests.Soft.Controllers; 
public abstract class ControllerBaseTests<TController, TObject, TData, TView> : 
        DbBaseTests<TController, BaseController<TObject, TData, TView>, TObject, TData> 
    where TController : BaseController<TObject, TData, TView>
    where TObject : Entity<TData> 
    where TData : EntityData<TData>, new()
    where TView : EntityView, new() {

     private TView? view;
     private TView? createView() {
         view = Random.Object<TView>();
         view.Id = nextId;
         return view;
     }
     [TestMethod] public void CreateTest() {
         var r = obj!.Create();
         isType(r, typeof(ViewResult));
     }
     [TestMethod] public async Task CreateViewTest() {
        void isInDb (bool inDb = false ) {
            var o = dbSet!.Find(view!.Id);
            if (inDb) notNull(o);
            else isNull(o);
        }
        createView();
        isInDb();
        var r = await obj!.Create(view!);
        isInDb(true);
        isType(r, typeof(RedirectToActionResult));
    }
     [TestMethod] public async Task EditTest() {
        createView();
        var r = await obj!.Edit(view!.Id);
        isType(r, typeof(ViewResult));
    }
    [TestMethod] public async Task EditViewTest() {
        var d1 = createData();
        var v2 = createView();
        v2!.Id = d1.Id;
        addToSet(d1);
        await obj!.Edit(v2.Id, v2);
        var d = dbSet!.Find(d1!.Id);
        validate(d, v2);
    }

    [TestMethod] public async Task EditViewNotFoundTest() {
        var d1 = createData();
        var v2 = createView();
        var v2Id = v2!.Id;
        v2!.Id = d1.Id;
        addToSet(d1);
        var r = await obj!.Edit(v2Id, v2);
        isType(r, typeof(NotFoundResult));
    }
    [TestMethod] public async Task DeleteTest() {
        createView();
        var r = await obj!.Delete(view!.Id);
        isType(r, typeof(ViewResult));
    }
    [TestMethod] public async Task DeleteConfirmedTest() {
        var d1 = createData();
        addToSet(d1);
        await obj!.DeleteConfirmed(d1.Id);
        var d = dbSet!.Find(d1!.Id);
        isNull(d);
    }
    [TestMethod] public async Task DetailsTest() {
        createView();
        var r = await obj!.Details(view!.Id);
        isType(r, typeof(ViewResult));
    }
    private List<TData> list = [];
    private async Task get(int pageIdx, 
        string? orderBy = null, string? filter = null, int? selectedId = null) {
        list = [.. dbSet!.ToList()];
        var r = await obj!.Index(pageIdx, orderBy, filter, selectedId);
        isType(r, typeof(ViewResult));
    }
   
     [TestMethod] public async Task IndexTest() {
         await get(0);
         foreach (var pi in typeof(TData).GetProperties()) {
            await get(3, pi.Name);
            await get(2, pi.Name + "_desc");
            var filter = pi!.GetValue(list[0])!.ToString();
            await get(0, pi.Name, filter);
            filter = pi!.GetValue(list[1])!.ToString();
            await get(0, pi.Name, filter, Random.UInt8(1, lastId));
        }
    }

    private void validate(TData? d, TView v) {
        var validated = 0;
        foreach (var pi in d!.GetType().GetProperties()) {
            var actual = pi.GetValue(d);
            var vpi = v.GetType().GetProperty(pi.Name);
            if (vpi is null) continue;
            var expected = vpi.GetValue(v);
            equal(actual, expected);
            ++validated;
        }
        equal(validated, d!.GetType().GetProperties().Length);
    }
}
