using Microsoft.AspNetCore.Mvc;
using RecipeMvc.Soft.Data;
using RecipeMvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using RecipeMvc.Facade.Account;

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
                if (UsernameOrEmailExists(model.Username, null))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(model);
                }
                if (UsernameOrEmailExists(null, model.Email))
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
        public async Task<IActionResult> Login(LoginView model)
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
                        await SignInUserAsync(user);
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

        [Authorize]
        public IActionResult Edit()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var user = _context.UserAccounts.FirstOrDefault(u => u.Username == username || u.Email == username);
            if (user == null)
                return RedirectToAction("Login");

            var model = new EditUserAccountView
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditUserAccountView model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.UserAccounts.FirstOrDefault(u => u.Id == model.Id);
            if (user == null)
                return RedirectToAction("Login");

            if (!string.Equals(user.Username, model.Username, StringComparison.OrdinalIgnoreCase) &&
                UsernameOrEmailExists(model.Username, null, model.Id))
            {
                ModelState.AddModelError("Username", "Username is already taken.");
                return View(model);
            }

            if (!string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase) &&
                UsernameOrEmailExists(null, model.Email, model.Id))
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Username = model.Username;

            _context.SaveChanges();
            await SignInUserAsync(user);
            return RedirectToAction("AccountInfo");
        }

        private bool UsernameOrEmailExists(string? username, string? email, int? excludeUserId = null)
        {
            return _context.UserAccounts.Any(u =>
                ((username != null && u.Username == username) ||
                 (email != null && u.Email == email)) &&
                (!excludeUserId.HasValue || u.Id != excludeUserId.Value)
            );
        }

        private async Task SignInUserAsync(UserAccountData user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new("FirstName", user.FirstName),
                new(ClaimTypes.Role, "User"),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );
        }
    }
}