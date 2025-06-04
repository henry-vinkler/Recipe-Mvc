using RecipeMvc.Data;
using RecipeMvc.Data.Entities;

namespace RecipeMvc.Tests.Data;

[TestClass] public class RecipeDataTests : SealedTests<RecipeData, EntityData<RecipeData>> {
    [TestInitialize] public override void Initialize() {
        base.Initialize();
        if (obj == null) return;
        obj.Id = 1;
        obj.AuthorId = 1;
        obj.Author = new UserAccountData();
        obj.Title = "Title";
        obj.Description = "This is description.";
        obj.ImagePath = "ImagePath.jpg";
        obj.Calories = 50f;
        obj.Tags = "Tag one, tag two";
    }

    [TestMethod] public void CloneTest() {
        var d = obj?.Clone();
        notNull(d);
        equal(obj.Id, d?.Id);
        equal(obj.AuthorId, d?.AuthorId);
        notNull(d?.Author);
        equal(obj.Title, d?.Title);
        equal(obj.Description, d?.Description);
        equal(obj.ImagePath, d?.ImagePath);
        equal(obj.Calories, d?.Calories);
        equal(obj.Tags, d?.Tags);
    }
    [TestMethod] public void TitleTest() => isProperty<string>();
    [TestMethod] public void DescriptionTest() => isProperty<string>();
    [TestMethod] public void ImagePathTest() => isProperty<string>();
    [TestMethod] public void CaloriesTest() => isProperty<float>();
    [TestMethod] public void TagsTest() => isProperty<string>();
}