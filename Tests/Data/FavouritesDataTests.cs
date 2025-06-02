using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeMvc.Data;

namespace RecipeMvc.Tests.Data;

[TestClass] public class FavouriteDataTests : SealedTests<FavouriteData, EntityData<FavouriteData>> {

    [TestInitialize]
    public override void Initialize() {
        base.Initialize();
        if (obj == null) return;
        obj.UserId = 2;
        obj.RecipeId = 5;
    }

    [TestMethod] public void UserIdTest() => isProperty<int>();
    [TestMethod] public void RecipeIdTest() => isProperty<int>();

    [TestMethod] public void CloneTest() {
        var d = obj?.Clone();
        notNull(d);
        equal(obj.UserId, d?.UserId);
        equal(obj.RecipeId, d?.RecipeId);
    }
}

