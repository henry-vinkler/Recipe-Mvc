namespace RecipeMvc.Domain;
public interface IRepo<TObject>
{
    public Task<int> PageCount(byte pageSize, string? filter);
    public Task<IEnumerable<TObject>> GetAsync(int pageIdx, byte pageSize,
        string? orderBy = null, string? filter = null);
    public Task<IEnumerable<TObject>> GetAsync(string propertyName, int idValue);
    public Task<IEnumerable<TObject>> GetAsync();
    public Task<TObject?> GetAsync(int? id);
    public Task AddAsync(TObject o);
    public Task UpdateAsync(TObject o);
    public Task DeleteAsync(int? id);
    public Task<IEnumerable<dynamic>> SelectItems(string searchString, int id);
    public Task<dynamic?> SelectItem(int id);
}
