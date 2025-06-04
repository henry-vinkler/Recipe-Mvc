using RecipeMvc.Data.Entities;
using RecipeMvc.Domain;

namespace RecipeMvc.Tests.Domain;

[TestClass] public class RecipeTests : BaseClassTests<Recipe, Entity<RecipeData>> {
    protected override Recipe createObj() {
        var d = new RecipeData {
            Id = 1,
            AuthorId = 2,
            Title = "Title",
            Description = "Description",
            ImagePath = "Image.jpg",
            Calories = 100f,
            Tags = "Tag1, Tag2"
        };
        return new Recipe(d);
    }

    [TestMethod] public void IdTest() => equal(1, obj?.Id);
    [TestMethod] public void AuthorIdTest() => equal(2, obj?.AuthorId);
    [TestMethod] public void TitleTest() => equal("Title", obj?.Title);
    [TestMethod] public void DescriptionTest() => equal("Description", obj?.Description);
    [TestMethod] public void ImagePathTest() => equal("Image.jpg", obj?.ImagePath);
    [TestMethod] public void CaloriesTest() => equal(100f, obj?.Calories);
    [TestMethod] public void TagsTest() => equal("Tag1, Tag2", obj?.Tags);
    [TestMethod] public void DataTest() => notNull(obj?.Data);
    [TestMethod] public void IdTypeTest() => isType(obj?.Id, typeof(int));
    [TestMethod] public void AuthorIdTypeTest() => isType(obj?.AuthorId, typeof(int));
    [TestMethod] public void TitleTypeTest() => isType(obj?.Title, typeof(string));
    [TestMethod] public void DescriptionTypeTest() => isType(obj?.Description, typeof(string));
    [TestMethod] public void ImagePathTypeTest() => isType(obj?.ImagePath, typeof(string));
}
