using RecipeMvc.Aids.GoF.Crea;

namespace RecipeMvc.Data;

public abstract class EntityData {
    public int Id { get; set; }
}

public abstract class EntityData<T> : EntityData, ICloneable<T> where T : EntityData<T> {
    public virtual T Clone() => (T) MemberwiseClone();
}
 

