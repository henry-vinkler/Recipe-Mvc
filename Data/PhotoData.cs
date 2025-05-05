
namespace RecipeMvc.Data;

public sealed class PhotoData : EntityData<PhotoData> {
    public int Id { get; set; } 
    public int RecipeId { get; set; } 
    public string FileName { get; set; } 
    public DateTime Uploaded { get; set; }
}
