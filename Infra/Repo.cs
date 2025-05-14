using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;
using RecipeMvc.Domain;

namespace RecipeMvc.Infra;
public sealed class UserAccountRepo(DbContext db)
    : Repo<UserAccount, UserAccountData>(db, d => new(d)), IUserAccountRepo {
    protected internal override string selectTextField => nameof(UserAccountData.Username);
}
public class Repo<TObject, TData>(DbContext c , Func<TData?, TObject> f)  
    where TObject: Entity<TData> 
    where TData: EntityData<TData> {
    private readonly DbContext db = c;
    private readonly DbSet<TData> set = c.Set<TData>();
    private static bool isAsc(string s) => !s.EndsWith("_desc");
    private static string propName(string s) => s.Replace("_desc", "");
    private IQueryable<TData> ordered(string? orderBy = null, string? filter = null) 
        => orderBy is null ? filtered(filter) : isAsc(orderBy)
            ? filtered(filter).OrderBy(propName(orderBy))
            : filtered(filter).OrderBy(propName(orderBy) + " descending");
    private IQueryable<TData> filtered(string? filter = null) 
        => filter is null 
            ? set
            : set.Where(whereExpr(), filter);
    private IQueryable<TData> filtered(string propertyName, int idValue)
        => set.Where(whereExpr(propertyName), idValue);
    private string whereExpr(string propertyName) {
        var filters = new List<string>();
        foreach (var p in typeof(TData).GetProperties())
        {
            if (p.Name != propertyName) continue;
            if (p.PropertyType != typeof(int)) continue;
            filters.Add($"({p.Name}==@0)");
        }
        return string.Join(" OR ", filters);
    }
    private string whereExpr() {  
        var filters = new List<string>();
        foreach( var p in typeof(TData).GetProperties()) {
            if (p.PropertyType == typeof(string)) filters.Add($"({p.Name} != null && {p.Name}.Contains(@0))");
            else if (p.PropertyType == typeof(DateTime)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(char)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(bool)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(byte)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(short)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            //else if (p.PropertyType == typeof(int)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(long)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(sbyte)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(ushort)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(uint)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(ulong)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(double)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(float)) filters.Add($"({p.Name}.ToString().Contains(@0))");
            else if (p.PropertyType == typeof(decimal)) filters.Add($"({p.Name}.ToString().Contains(@0))");
        }
        return string.Join(" OR ", filters);
    }
    public async Task<int> PageCount(byte pageSize, string? filter) {
        var cnt = await filtered(filter).CountAsync();
        return cnt % pageSize == 0? cnt/pageSize: cnt/pageSize + 1;   
    }
    public async Task<IEnumerable<TObject>> GetAsync(string propertyName, int idValue)
        => (await filtered(propertyName, idValue).ToListAsync()).Select(f);
    public async Task<IEnumerable<TObject>> GetAsync(int pageIdx, byte pageSize
        , string? orderBy=null, string? filter=null) 
        => (await ordered(orderBy, filter)
            .Skip(pageIdx*pageSize).Take(pageSize).ToListAsync()).Select(f);
    public async Task<IEnumerable<TObject>> GetAsync() => (await set.ToListAsync()).Select(f);
    public async Task<TObject?> GetAsync(int? id) => id is null? null: f(await set.FindAsync(id));
    public async Task AddAsync(TObject o) {
        var d = o?.Data;
        if (d is null) return;
        set.Add(d);
        await db.SaveChangesAsync();
    }
    public async Task UpdateAsync(TObject o) {
        var d = o?.Data;
        if (d is null) return;
        set.Update(d);
        await db.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id) {
        var x = await set.FindAsync(id);
        if (x is null) return;
        set.Remove(x);
        await db.SaveChangesAsync();
    }
    protected internal virtual string selectTextField => nameof(EntityData.Id);
    public async Task<IEnumerable<dynamic>> SelectItems(string searchString, int id) {
        var l = (await GetAsync()).ToList();
        var o = await GetAsync(id);
        if (o != null && !l.Contains(o)) l.Add(o);
        return new SelectList(l, nameof(EntityData.Id), selectTextField, id);
    }
    private async Task<SelectList> selectList(int id, List<TObject>? l = null) {
        var list = l ?? [];
        var o = await GetAsync(id);
        if (o != null && !list.Contains(o)) list.Add(o);
        return new SelectList(list, nameof(EntityData.Id), selectTextField, id);
    }
    public async Task<dynamic?> SelectItem(int id) {
        var l = await selectList(id);
        return l.FirstOrDefault();
    }
}