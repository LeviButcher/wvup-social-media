using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.WebServiceAccess.Base;


namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IWebApiCalls _webApiCalls;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IWebApiCalls webApiCalls, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _webApiCalls = webApiCalls;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult AdminMenu()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetAccount()
        {
            return View();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> ChangePassword(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("ResetAccount", _userManager.Users);
            }

            var model = new UserProfileWithPassword() { Id = user.Id, UserName = user.UserName, Email = user.Email };

            return View(model);
        }

        [HttpPost("{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string userId, UserProfileWithPassword model)
        {
            if (!ModelState.IsValid && model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords Must Match");
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            await _userManager.RemovePasswordAsync(user);

            var change = await _userManager.AddPasswordAsync(user, model.ConfirmPassword);
            if (!change.Succeeded)
            {
                return View(model);
            }

            return RedirectToAction("ResetAccount");
        }
    }
}
