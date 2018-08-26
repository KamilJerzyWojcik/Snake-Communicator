using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snake.Context;
using Snake.Models;
using Snake.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Snake.Controllers
{
    public class AccountController : Controller
    {

        protected UserManager<UserModel> UserManager { get; }
        protected SignInManager<UserModel> SignInManager { get; }
        protected RoleManager<IdentityRole<int>> RoleManager { get; }
        private readonly EFContext _context;

        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, RoleManager<IdentityRole<int>> roleManager, EFContext context)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            _context = context;
        }


        public async Task<IActionResult> AddRole()
        {
            var ir = new IdentityRole<int>("Admin");
            await RoleManager.CreateAsync(ir);
            return Content("Dodano rolę!");
        }

        public async Task<IActionResult> GetUser()
        {
            UserModel user = await UserManager.GetUserAsync(User);
            return View(user);
        }

        public async Task<IActionResult> AddUserToRole()
        {
            UserModel user = await UserManager.GetUserAsync(User);
            user = await UserManager.GetUserAsync(User);
            await UserManager.AddToRoleAsync(user, "Admin");
            return Content($"Dodano użytkownika {User.Identity.Name}");
        }

        public async Task<IActionResult> RemoveUserFromRole()
        {
            UserModel user = await UserManager.GetUserAsync(User);

            user = await UserManager.GetUserAsync(User);
            await UserManager.RemoveFromRoleAsync(user, "Admin");
            return RedirectToAction("Index", "Home");
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel(viewModel.Login) { Email = viewModel.Email };
                var result = await UserManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    await SignInManager.PasswordSignInAsync(viewModel.Login,
                            viewModel.Password, true, false);

                    return RedirectToAction("Index", "Home");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(viewModel.Login,
                    viewModel.Password, viewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Niepoprawny login lub hasło");
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
