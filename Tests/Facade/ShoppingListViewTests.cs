﻿using RecipeMvc.Facade;
using RecipeMvc.Facade.ShoppingList;

namespace RecipeMvc.Tests.Facade;

[TestClass]
public class ShoppingListViewTests : SealedTests<ShoppingListView, EntityView>
{
    [TestMethod] public override void DisplayNameTest() => isDisplayName("Shopping list");
    [TestMethod] public void IdTest() => isProperty<int>();
    [TestMethod] public void UserIdTest() => isProperty<int>();
    [TestMethod] public void NameTest() => isProperty<string>();
    [TestMethod] public void NotesTest() => isProperty<string>();
    [TestMethod] public void IsCheckedTest() => isProperty<bool>();
    [TestMethod] public void IngredientsTest() => isProperty<IList<ShoppingListIngredientView>>();
}
