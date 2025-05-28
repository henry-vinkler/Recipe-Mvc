using RecipeMvc.Data;

namespace RecipeMvc.Tests.Data;

[TestClass] public class EntityDataIdTests : AbstractTests<EntityData, object>{
    protected override EntityData createObj() => new RecipeData();
    [TestMethod] public void IdTest() => isProperty<int>();
}
