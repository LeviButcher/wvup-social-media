using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.Models;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IWebApiCalls _webApiCalls;

        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }

        public HomeController(IWebApiCalls webApiCalls, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _webApiCalls = webApiCalls;
            UserManager = userManager;
            SignInManager = signInManager;
        }


        [HttpGet]
        [Route("~/")]
        public async Task<IActionResult> Index()
        {
            if (!SignInManager.IsSignedIn(User)) return RedirectToAction("Login");

            var user = await UserManager.GetUserAsync(User);
            IList<UserPost> posts = await _webApiCalls.GetFollowingPostAsync(user.Id);

            return View(posts);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string search)
        {
            var users = await _webApiCalls.SearchUserAsync(search);
            ViewData["Title"] = $"Search:{search}";
            return View("UserList", users);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null) return View(model);

            var result = await SignInManager.PasswordSignInAsync(user, model.password, false, false);

            if (!result.Succeeded) return View(model);

            return RedirectToAction("Index");
        }

        [Route("~/Logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel register)
        {
            if (!ModelState.IsValid) return View(register);

            if (register.Password != register.ConfirmPassword) return View(register);

            User user = new User()
            {
                Email = register.Email,
                UserName = register.UserName
            };

            var result = await _webApiCalls.CreateUserAsync(register.Password, user);
            var resultUser = JsonConvert.DeserializeObject<User>(result);

            if (resultUser == null) return View(register);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            User user = await UserManager.GetUserAsync(HttpContext.User);
            UserProfile userProfile = await _webApiCalls.GetUserAsync(user.Id);

            return View(userProfile);
        }
    }
}
