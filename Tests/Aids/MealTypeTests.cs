using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeMvc.Aids;
using RecipeMvc.Tests;

namespace RecipeMvc.Tests.Aids;

[TestClass]
public class MealTypeTests : EnumTests<MealType>
{
    public MealTypeTests() : base(5) { }

    [TestMethod] public void BreakfastTest() => isEnum((int)MealType.Breakfast);
    [TestMethod] public void LunchTest() => isEnum((int)MealType.Lunch);
    [TestMethod] public void DinnerTest() => isEnum((int)MealType.Dinner);
    [TestMethod] public void SnackTest() => isEnum((int)MealType.Snack);
    [TestMethod] public void DessertTest() => isEnum((int)MealType.Dessert);
}
