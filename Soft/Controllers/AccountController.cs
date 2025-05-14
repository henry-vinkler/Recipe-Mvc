using Microsoft.AspNetCore.Mvc;
using RecipeMvc.Soft.Data;
using RecipeMvc.Facade;
using RecipeMvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace RecipeMvc.Soft.Controllers
{
    public class AccountController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(RegistrationView model)
        {
            if (ModelState.IsValid)
            {
                // Check for existing username or email
                bool usernameExists = _context.UserAccounts.Any(u => u.Username == model.Username);
                bool emailExists = _context.UserAccounts.Any(u => u.Email == model.Email);

                if (usernameExists)
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(model);
                }
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(model);
                }

                var passwordHasher = new PasswordHasher<UserAccountData>();
                UserAccountData account = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username
                };
                account.Password = passwordHasher.HashPassword(account, model.Password);

                try
                {
                    _context.UserAccounts.Add(account);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName}. Registration successful! Please log in now";
                    return View();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                    return View(model);
                }
            }
            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginView model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserAccounts
                    .FirstOrDefault(u => u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail);

                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<UserAccountData>();
                    var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        var claims = new List<Claim>
                        {
                            new(ClaimTypes.Name, user.FirstName),
                            new("Name", user.FirstName),
                            new(ClaimTypes.Role, "User"),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username/email or password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username/email or password.");
                }
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult AccountInfo()
        {
            ViewData["Title"] = "AccountInfo";

            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            var user = _context.UserAccounts.FirstOrDefault(u => u.Username == username || u.Email == username);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }
    }
}
