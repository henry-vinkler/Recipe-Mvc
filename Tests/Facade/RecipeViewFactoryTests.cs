using RecipeMvc.Data.Entities;
using RecipeMvc.Facade;
using RecipeMvc.Facade.Recipe;

namespace RecipeMvc.Tests.Facade;

[TestClass] public class RecipeViewFactoryTests :
    SealedTests<RecipeViewFactory, AbstractViewFactory<RecipeData, RecipeView>>{ }