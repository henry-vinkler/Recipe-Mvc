using RecipeMvc.Facade.Recipe;
using RecipeMvc.Facade;
using RecipeMvc.Data;
using RecipeMvc.Aids;

namespace RecipeMvc.Tests.Facade;

[TestClass] public class RecipeViewFactoryTests : SealedTests<RecipeViewFactory, AbstractViewFactory<RecipeData, RecipeView>> {
    private RecipeData? data;
    private RecipeView? view;

    [TestInitialize] public override void Initialize() {
        base.Initialize();
        data = crData();
        view = crView();
    }
    [TestCleanup] public override void Cleanup() {
        base.Cleanup();
        data = null;
        view = null;
    }
    private RecipeView crView() {
        return new RecipeView {
            Id = 1,
            AuthorId = 2,
            AuthorUsername = "UserName",
            Title = "viewTitle",
            Description = "viewDescription",
            Calories = 100f,
            Tags = "Tag 1"
        };
    }
    private RecipeData crData() {
        return new RecipeData {
            Id = 10,
            AuthorId = 20,
            Title = "Title",
            Description = "Description",
            Calories = 200f,
            Tags = "Tag1, Tag2"
        };
    }
    [TestMethod] public void CreateViewTest() {
        var f = new RecipeViewFactory();
        var v = f.CreateView(data);
        notNull(v);
        equal(data?.Id, v.Id);
        equal(data?.AuthorId, v.AuthorId);
        equal(data?.Title, v.Title);
        equal(data?.Description, v.Description);
        equal(data?.Calories, v.Calories);
        equal(data?.Tags, v.Tags);
    }
    [TestMethod] public void CreateDataTest() {
        var f = new RecipeViewFactory();
        var d = f.CreateData(view);
        notNull(d);
        equal(view?.Id, d.Id);
        equal(view?.AuthorId, d.AuthorId);
        equal(view?.Title, d.Title);
        equal(view?.Description, d.Description);
        equal(view?.Calories, d.Calories);
        equal(view?.Tags, d.Tags);
    }
}