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
    [Authorize]
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

        /// <summary>
        ///     Site Index Page
        /// </summary>
        [HttpGet]
        [Route("~/")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (!SignInManager.IsSignedIn(User)) return RedirectToAction("Login");

            var user = await UserManager.GetUserAsync(User);
            IList<UserPost> posts = await _webApiCalls.GetFollowingPostAsync(user.Id);
            
            return View(posts);
        }

        /// <summary>
        ///     Loads follow feed
        /// </summary>
        /// <param name="userId">UserId to determine who's followers to get posts</param>
        /// <param name="skip">From URL Query, determines the number of posts to retrieve, default is 0
        /// <param name="take">From URL Query, determines the number of posts to retrieve, default is 10
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFollowingPost(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(await _webApiCalls.GetFollowingPostAsync(userId, skip, take));
        }

        /// <summary>
        ///     Error page
        /// </summary>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        ///     Search View which will display a list of users or groups that match search term
        ///     depending on which tab is selected within view
        /// </summary>
        /// <param name="search">From URL Query, term to search for partial matches 
        /// <param name="tab">From URL Query, determines whether to display users or groups
        /// Tabs are: Users, Groups</param>
        [HttpGet]
        public IActionResult Search([FromQuery] string search, [FromQuery] string tab)
        {
            ViewData["Title"] = $"Search:{search}";
            ViewData["Term"] = search;
            ViewData["tab"] = tab ?? "";
            return View();
        }

        /// <summary>
        ///    Initial view a user will be presented with when visiting site
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        ///    HttpPost of LoginView
        /// </summary>
        /// <param name="model"> LoginViewModel, used to determine if User is in database and if 
        /// the entered password matches stored password.  if user is registered and enters correct password, 
        /// user will be redirect to their Index page, else directed to re-enter email and password
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null) return View(model);

            var result = await SignInManager.PasswordSignInAsync(user, model.password, false, false);

            if (!result.Succeeded) return View(model);

            return RedirectToAction("Index");
        }

        /// <summary>
        ///  Action will Log user out of system and redirect to login page
        /// </summary>
        [Route("~/Logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        /// <summary>
        ///  View to register new User
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registration()
        {
            return View();
        }


        /// <summary>
        ///  View will allow user to register to WVUPSM
        /// </summary>
        /// <param name="register"> RegisterViewModel, user must enter a valid email, username, and password
        /// An email is then sent to confirm the User's email address, only after confirming, User will 
        /// be able to LogIn to system
        [HttpPost]
        [AllowAnonymous]
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

                TempData["Announcement"] = $"An Email Confirmation has been sent to {register.Email}";
                return View("Login");

            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(register);
        }

        /// <summary>
        ///  When User clicks link in Email, this action is called, if User and Code match, User's email will
        ///  be confirmed
        /// </summary>
        /// <param name="userId">From URL Query, userId of newly registered user
        /// <param name="code">From URL Query, auto-generated code from confirmation email
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

        /// <summary>
        ///  Settings for customizing user profile, etc.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Settings()
        {
            User user = await UserManager.GetUserAsync(HttpContext.User);
            UserProfile userProfile = await _webApiCalls.GetUserAsync(user.Id);

            return View(userProfile);
        }
    }
}
