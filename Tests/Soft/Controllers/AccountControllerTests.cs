using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeMvc.Data.DbContext;
using RecipeMvc.Data.Entities;
using RecipeMvc.Facade.Account;
using RecipeMvc.Soft.Controllers;
using RecipeMvc.Infra;
using RecipeMvc.Domain;

namespace RecipeMvc.Tests.Soft.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        private ApplicationDbContext dbContext;
        private IUserAccountRepo userRepo;
        private AccountController controller;
        private List<UserAccountData> users = new();

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            dbContext = new ApplicationDbContext(options);
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            userRepo = new UserAccountRepo(dbContext);
            SeedUsers(1);

            controller = new AccountController(userRepo);

            // Set up a fake authenticated user for actions that require it
            var user = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "testuser1")
                }, "mock"));

            var httpContext = new DefaultHttpContext { User = user };

            // Register authentication, TempData, and UrlHelper services
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddAuthentication("Cookies")
                .AddCookie("Cookies");
            services.AddAuthorization();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ITempDataProvider, TempDataProviderStub>();
            services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            httpContext.RequestServices = services.BuildServiceProvider();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            controller.TempData = new TempDataDictionary(httpContext, httpContext.RequestServices.GetService<ITempDataProvider>());

            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            var urlHelperFactory = httpContext.RequestServices.GetService<IUrlHelperFactory>();
            controller.Url = urlHelperFactory?.GetUrlHelper(actionContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Database.CloseConnection();
            dbContext.Dispose();
        }

        private void SeedUsers(int count = 1)
        {
            users.Clear();
            for (int i = 1; i <= count; i++)
            {
                users.Add(new UserAccountData
                {
                    Id = i,
                    FirstName = $"Test{i}",
                    LastName = $"User{i}",
                    Email = $"test{i}@example.com",
                    Username = $"testuser{i}",
                    Password = new Microsoft.AspNetCore.Identity.PasswordHasher<UserAccountData>().HashPassword(null, "password")
                });
            }
            dbContext.UserAccounts.AddRange(users);
            dbContext.SaveChanges();
        }

        [TestMethod]
        public void CanCallRegistrationGet()
        {
            var result = controller.Registration();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Registration_Successful()
        {
            var model = new RegistrationView
            {
                FirstName = "New",
                LastName = "User",
                Email = "new@example.com",
                Username = "newuser",
                Password = "password",
                ConfirmPassword = "password"
            };
            var actionResult = await controller.Registration(model);
            var result = actionResult as RedirectToActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public async Task Registration_UsernameTaken()
        {
            var model = new RegistrationView
            {
                FirstName = "Test",
                LastName = "User",
                Email = "unique@example.com",
                Username = "testuser1",
                Password = "password",
                ConfirmPassword = "password"
            };
            var actionResult = await controller.Registration(model);
            var result = actionResult as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public async Task Login_Successful()
        {
            var model = new LoginView
            {
                UsernameOrEmail = "testuser1",
                Password = "password"
            };
            var actionResult = await controller.Login(model);
            var result = actionResult as RedirectToActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public async Task Login_Failure()
        {
            var model = new LoginView
            {
                UsernameOrEmail = "testuser1",
                Password = "wrongpassword"
            };
            var actionResult = await controller.Login(model);
            var result = actionResult as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public async Task Logout_RedirectsToHome()
        {
            var actionResult = await controller.Logout();
            var result = actionResult as RedirectToActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public async Task AccountInfo_AuthenticatedUser()
        {
            var actionResult = await controller.AccountInfo();
            var result = actionResult as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(UserAccount));
        }

        [TestMethod]
        public async Task Edit_Get_ReturnsView()
        {
            var actionResult = await controller.Edit();
            var result = actionResult as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(EditUserAccountView));
        }

        [TestMethod]
        public async Task Edit_Post_UpdatesUser()
        {
            var model = new EditUserAccountView
            {
                Id = 1,
                FirstName = "Updated",
                LastName = "User",
                Email = "test1@example.com",
                Username = "testuser1"
            };
            var actionResult = await controller.Edit(model);
            var result = actionResult as RedirectToActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("AccountInfo", result.ActionName);
            var updated = dbContext.UserAccounts.Find(1);
            Assert.AreEqual("Updated", updated.FirstName);
        }

        [TestMethod]
        public async Task Delete_Get_ReturnsView()
        {
            var actionResult = await controller.Delete(1);
            var result = actionResult as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(DeleteUserAccountView));
        }

        [TestMethod]
        public async Task Delete_Post_CorrectPassword_DeletesUser()
        {
            var model = new DeleteUserAccountView
            {
                Id = 1,
                Password = "password"
            };
            var actionResult = await controller.Delete(model);
            var result = actionResult as RedirectToActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            var deleted = dbContext.UserAccounts.Find(1);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public async Task Delete_Post_WrongPassword_ShowsError()
        {
            var model = new DeleteUserAccountView
            {
                Id = 1,
                Password = "wrongpassword"
            };
            var actionResult = await controller.Delete(model);
            var result = actionResult as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            var stillExists = dbContext.UserAccounts.Find(1);
            Assert.IsNotNull(stillExists);
        }

        // In-memory TempData provider for tests
        private class TempDataProviderStub : ITempDataProvider
        {
            private Dictionary<string, object> _data = new();
            public IDictionary<string, object> LoadTempData(HttpContext context) => _data;
            public void SaveTempData(HttpContext context, IDictionary<string, object> values) => _data = new(values);
        }
    }
}