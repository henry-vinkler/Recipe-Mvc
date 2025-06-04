using RecipeMvc.Data;
using RecipeMvc.Data.Entities;

namespace RecipeMvc.Tests.Data;

[TestClass] public class EntityDataTests:AbstractTests<EntityData<RecipeData>, EntityData> {
    protected override EntityData<RecipeData> createObj() => new RecipeData();
    [TestInitialize] public override void Initialize() {
        base.Initialize();
        if (obj== null) return;
        var o = obj as RecipeData;
        o!.Id = 1;
        o!.AuthorId = 1;
        o!.Author = new UserAccountData();
        o!.Title = "Title";
        o!.Description = "This is description.";
        o!.ImagePath = "ImagePath.jpg";
        o!.Calories = 50f;
        o!.Tags = "Tag one, tag two";
    }
    [TestMethod] public void CloneTest() {
        var d = obj?.Clone();
        notNull(d);
        equal(1, d?.Id);
        equal(1, d?.AuthorId);
        notNull(d?.Author);
        equal("Title", d?.Title);
        equal("This is description.", d?.Description);
        equal("ImagePath.jpg", d?.ImagePath);
        equal(50f, d?.Calories);
        equal("Tag one, tag two", d?.Tags);
    }
}
