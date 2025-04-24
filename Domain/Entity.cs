using RecipeMvc.Data;

namespace RecipeMvc.Domain;

public class Entity<TData>(TData? d) where TData: EntityData<TData> {
    public TData? Data { get; } = d?.Clone();
    public int? Id => Data?.Id;
}


 
