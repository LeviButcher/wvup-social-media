using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public IEmailSender EmailSender { get; }

        public HomeController(IWebApiCalls webApiCalls, UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _webApiCalls = webApiCalls;
            UserManager = userManager;
            SignInManager = signInManager;
            EmailSender = emailSender;
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFollowingPost(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(await _webApiCalls.GetFollowingPostAsync(userId, skip, take));
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

            var result = await UserManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Home",
                    values: new { userId = user.Id, code },
                    protocol: Request.Scheme);



                await EmailSender.SendEmailAsync(register.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToAction("Login");

            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(register);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            {
                if (userId == null || code == null)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                var user = await UserManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{userId}'.");
                }
                var result = await UserManager.ConfirmEmailAsync(user, code);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }
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
