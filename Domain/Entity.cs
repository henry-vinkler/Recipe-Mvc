using RecipeMvc.Core;
using RecipeMvc.Data.Entities;

namespace RecipeMvc.Domain;

public abstract class Entity<TData>(TData? d) : IEntity where TData : EntityData<TData> {
    public TData? Data { get; } = d?.Clone();
    public int? Id => Data?.Id;
    public virtual async Task LoadLazy() => await Task.CompletedTask;
    internal protected static async Task<TItem?> getItem<TRepo, TItem>(int id)
          where TRepo : IRepo<TItem> where TItem : class, IEntity
          => await Services.GetItem<TRepo, TItem>(id);
    internal protected static async Task<TItem?> getItem<TRepo, TItem>(string propertyName, int id)
          where TRepo : IRepo<TItem> where TItem : class, IEntity
          => await Services.GetItem<TRepo, TItem>(propertyName, id);
    internal protected static async Task<IEnumerable<TItem?>> getList<TRepo, TItem>(string propertyName, int id)
          where TRepo : IRepo<TItem> where TItem : class, IEntity
          => await Services.GetList<TRepo, TItem>(propertyName, id);
}


 
