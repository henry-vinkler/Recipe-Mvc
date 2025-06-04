using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using RecipeMvc.Data.Entities;
using Random = RecipeMvc.Aids.Random;

namespace RecipeMvc.Tests;

internal class TestHost<TPrg, TDb> : WebApplicationFactory<TPrg> 
    where TPrg: class where TDb: DbContext{
    internal protected TDb? db;
    protected override void ConfigureWebHost(IWebHostBuilder b) {
        b.UseEnvironment("Testing")
        .ConfigureServices(s => {
            removeDatabase<TDb>(s);
            addInMemoryDatabase<TDb>(s);
        });
    }
    private void removeDatabase<T>(IServiceCollection c) where T : DbContext {
        var d = c.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<T>));
        if (d != null) { c.Remove(d); }
    }
    private void addInMemoryDatabase<T>(IServiceCollection c) where T : DbContext 
        => c.AddDbContext<T>(o => o.UseSqlite($"DataSource=file:TestDb_{Guid.NewGuid()};mode=memory;cache=shared"));
    internal TDb? startDb() {
       var scope = Services.CreateScope();
       db = scope.ServiceProvider.GetRequiredService<TDb>();
       db.Database.EnsureDeleted();
       db.Database.EnsureCreated();
       return db;
   }
    internal HttpClient? startClient() => CreateClient(new WebApplicationFactoryClientOptions {
        AllowAutoRedirect = false
    });
    internal byte lastId;
    internal byte nextId => ++lastId;
    internal TData createData<TData>() where TData: EntityData<TData>, new()  {
       var d = Random.Object<TData>();
       d.Id = nextId;
       return d;
    }
    private static readonly object seedLock = new object();
    internal void seedData<TData>() where TData: EntityData<TData>, new() {
        lock (seedLock) {
            var set = db?.Set<TData>();
            var l = new List<TData>();
            for (var i = 0; i < Random.UInt8(20, 30); i++) l.Add(createData<TData>());
            set?.AddRange(l);
            save();
        }
    }
    internal void addToSet<TData>(TData d) where TData : EntityData<TData>, new() {
        lock (seedLock) {
            var set = db?.Set<TData>();
            set?.Add(d);
            save();
        }
    }
    private void save() {
        try {
            db?.SaveChanges();
        } catch (Exception e) {
            Assert.Fail(e.Message);
        }
    }
}
