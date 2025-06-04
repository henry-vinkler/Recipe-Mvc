using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using RecipeMvc.Domain;
using RecipeMvc.Data.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using RecipeMvc.Facade.Account;
using RecipeMvc.Infra;

namespace RecipeMvc.Soft.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserAccountRepo _userRepo;

        public AccountController(IUserAccountRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationView model)
        {
            if (ModelState.IsValid)
            {
                if (await UsernameOrEmailExists(model.Username, null))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(model);
                }
                if (await UsernameOrEmailExists(null, model.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(model);
                }

                var userAccountData = new UserAccountData
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username
                };
                var userAccount = new UserAccount(userAccountData);
                var passwordHasher = new PasswordHasher<UserAccount>();
                userAccount.Password = passwordHasher.HashPassword(userAccount, model.Password);

                try
                {
                    await _userRepo.AddAsync(userAccount);
                    var createdUser = await _userRepo.GetByUsernameOrEmailAsync(model.Username);
                    if (createdUser != null)
                    {
                        await SignInUserAsync(createdUser);
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Registration failed. Please try again.");
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while saving. Please try again.");
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
                var user = await _userRepo.GetByUsernameOrEmailAsync(model.UsernameOrEmail);
                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<UserAccount>();
                    var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        await SignInUserAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid username/email or password.");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> AccountInfo()
        {
            ViewData["Title"] = "AccountInfo";
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var user = await _userRepo.GetByUsernameOrEmailAsync(username);
            if (user == null)
                return RedirectToAction("Login");

            await user.LoadLazy();
            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var user = await _userRepo.GetByUsernameOrEmailAsync(username);
            if (user == null)
                return RedirectToAction("Login");

            var model = new EditUserAccountView
            {
                Id = user.Id ?? 0,
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

            var user = await _userRepo.GetAsync(model.Id);
            if (user == null)
                return RedirectToAction("Login");

            if (!string.Equals(user.Username, model.Username, System.StringComparison.OrdinalIgnoreCase) &&
                await UsernameOrEmailExists(model.Username, null, model.Id))
            {
                ModelState.AddModelError("Username", "Username is already taken.");
                return View(model);
            }

            if (!string.Equals(user.Email, model.Email, System.StringComparison.OrdinalIgnoreCase) &&
                await UsernameOrEmailExists(null, model.Email, model.Id))
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Username = model.Username;

            await _userRepo.UpdateAsync(user);
            await SignInUserAsync(user);
            return RedirectToAction("AccountInfo");
        }

        private async Task<bool> UsernameOrEmailExists(string? username, string? email, int? excludeUserId = null)
        {
            var users = await _userRepo.GetAsync();
            return users.Any(u =>
                ((username != null && u.Username == username) ||
                 (email != null && u.Email == email)) &&
                (!excludeUserId.HasValue || u.Id != excludeUserId.Value)
            );
        }

        private async Task SignInUserAsync(UserAccount user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, (user.Id ?? 0).ToString()),
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var username = User.Identity?.Name;
            var user = await _userRepo.GetAsync(id);
            if (user == null || (user.Username != username && user.Email != username))
                return RedirectToAction("Login");

            var model = new DeleteUserAccountView { Id = user.Id ?? 0 };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteUserAccountView model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var username = User.Identity?.Name;
            var user = await _userRepo.GetAsync(model.Id);
            if (user == null || (user.Username != username && user.Email != username))
                return RedirectToAction("Login");

            var passwordHasher = new PasswordHasher<UserAccount>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("Password", "Incorrect password.");
                return View(model);
            }

            await _userRepo.DeleteAsync(user.Id ?? 0);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}