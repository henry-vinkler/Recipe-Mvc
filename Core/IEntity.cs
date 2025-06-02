namespace RecipeMvc.Core;
public interface IEntity {
    public int? Id { get; }
    public Task LoadLazy();
}
