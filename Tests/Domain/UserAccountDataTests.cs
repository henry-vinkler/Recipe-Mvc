using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeMvc.Data;

namespace RecipeMvc.Tests.Domain
{
    [TestClass]
    public class UserAccountDataTests
    {
        private UserAccountData user;

        [TestInitialize]
        public void Setup()
        {
            user = new UserAccountData
            {
                Id = 42,
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com",
                Username = "alicesmith",
                Password = "hashedpassword"
            };
        }

        [TestMethod] public void IdTest() => Assert.AreEqual(42, user.Id);
        [TestMethod] public void FirstNameTest() => Assert.AreEqual("Alice", user.FirstName);
        [TestMethod] public void LastNameTest() => Assert.AreEqual("Smith", user.LastName);
        [TestMethod] public void EmailTest() => Assert.AreEqual("alice@example.com", user.Email);
        [TestMethod] public void UsernameTest() => Assert.AreEqual("alicesmith", user.Username);
        [TestMethod] public void PasswordTest() => Assert.AreEqual("hashedpassword", user.Password);
    }
}