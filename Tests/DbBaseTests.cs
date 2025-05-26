using Microsoft.EntityFrameworkCore;

using RecipeMvc.Data;
using RecipeMvc.Domain;
using RecipeMvc.Soft.Data;
using Random = RecipeMvc.Aids.Random;

namespace RecipeMvc.Tests;

public abstract class DbBaseTests<TClass, TBaseClass, TObject, TData>: 
        BaseClassTests<TClass, TBaseClass> 
    where TClass : class
    where TBaseClass : class 
    where TObject : Entity<TData> 
    where TData : EntityData<TData>, new() {
    protected ApplicationDbContext? dbContext;
    protected DbSet<TData>? dbSet;
    protected TObject? entity;
    internal byte lastId;
    internal byte nextId => ++lastId;
    internal protected TData createData(){
        var d = Random.Object<TData>();
        d.Id = nextId;
        return d;
    }
    protected abstract TObject? createEntity(Func<TData> getData);
    protected TObject? createEntity() => entity = createEntity(createData);
    [TestInitialize] public override void Initialize() {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        dbContext = new ApplicationDbContext(options);
        dbContext.Database.OpenConnection(); // For in-memory SQLite
        dbContext.Database.EnsureCreated();  // Ensure tables exist
        dbSet = dbContext!.Set<TData>();
        seedData();
        base.Initialize();
    }
    [TestCleanup] public override void Cleanup() {
       base.Cleanup();
       dbContext = null;
       dbSet = null;
       entity = null;
   }
    [TestMethod] public virtual void IsSealedTest() => isTrue(typeof(TClass).IsSealed);
    [TestMethod] public void CanCreateDbContextTest() => notNull(dbContext);
    [TestMethod] public void HasDbSetTest() => notNull(dbSet);
    [TestMethod] public void DbSetHasDataTest() => isTrue(dbSet!.Any());
    internal protected void seedData(){
         var l = new List<TData>();
         for (var i = 0; i < Random.UInt8(20, 30); i++) l.Add(createData());
         dbSet?.AddRange(l);
         dbContext?.SaveChanges();
         dbContext?.ChangeTracker.Clear();
     }
    internal protected void addToSet(TData d) {
         dbSet?.Add(d);
         dbContext?.SaveChanges();
         dbContext?.ChangeTracker.Clear();
     }
}
