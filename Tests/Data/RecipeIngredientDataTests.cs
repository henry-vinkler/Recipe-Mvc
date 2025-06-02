using RecipeMvc.Data;

namespace RecipeMvc.Tests.Data;

[TestClass]
public class RecipeIngredientDataTests : SealedTests<RecipeIngredientData, EntityData<RecipeIngredientData>> {

    [TestInitialize]
    public override void Initialize() {
        base.Initialize();
        if (obj == null) return;

        obj.Id = 1;
        obj.RecipeId = 10;
        obj.IngredientId = 5;
        obj.Quantity = 2.5f;
    }


    [TestMethod]
    public void RecipeIdTest() => isProperty<int>();

    [TestMethod]
    public void IngredientIdTest() => isProperty<int>();

    [TestMethod]
    public void QuantityTest() => isProperty<float>();

    [TestMethod]
    public void UnitTest() => isProperty<string>();
}
