using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.Models;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IWebApiCalls _webApiCalls;
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }

        public UserController(IWebApiCalls webApiCalls, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _webApiCalls = webApiCalls;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Index(string userId)
        {
            var user = await _webApiCalls.GetUserAsync(userId);
            var posts = await _webApiCalls.GetMyPostAsync(userId);
            ViewBag.Posts = posts;
            return View(user);
        }

        

        //Display page with all users Followers
        [HttpGet("{userId}")]
        public async Task<IActionResult> Followers(string userId)
        {
            ViewData["Title"] = "Followers";
            var followers = await _webApiCalls.GetFollowersAsync(userId);
            return View("UserList", followers);
        }

        //Display page with all users Following
        [HttpGet("{userId}")]
        public async Task<IActionResult> Following(string userId)
        {
            ViewData["Title"] = "Following";
            var following = await _webApiCalls.GetFollowingAsync(userId);
            return View("UserList", following);
        }

        //Unfollow and follow user
        [HttpPost("{userId}/{followId}")]
        public async Task<IActionResult> ToggleFollow(string userId, string followId)
        {
            var isFollowing = await  _webApiCalls.IsFollowingAsync(userId, followId);

            if (isFollowing)
            {
                await _webApiCalls.DeleteFollowAsync(userId, followId);
                return NoContent();
            }
            else
            {
                await _webApiCalls.CreateFollowAsync(new Follow()
                {
                    UserId = userId,
                    FollowId = followId
                });
                return Ok();
            }
        }

        [HttpGet("{userId}/{followId}")]
        public async Task<IActionResult> IsFollowing(string userId, string followId)
        {
            var result = await _webApiCalls.IsFollowingAsync(userId, followId);
            return Ok(result);
        }

        //Delete Confirmation Page
        [HttpGet("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            UserProfile userProfile = await _webApiCalls.GetUserAsync(userId);
            return View(userProfile);
        }

        [HttpPost("{userId}/{confirmDelete}")]
        public async Task<IActionResult> Delete(string userId,  bool confirmDelete, UserProfile userProfile)
        {
            if(!confirmDelete)
            {
                return RedirectToAction("Index");
            }
            User user = await UserManager.FindByIdAsync(userId);
            
            List<UserProfile> following = (List<UserProfile>) await _webApiCalls.GetFollowingAsync(userId);

            foreach(UserProfile users in following)
            {
               await _webApiCalls.DeleteFollowAsync(userId, users.UserId);
            }

            await SignInManager.SignOutAsync();
            var result = await UserManager.DeleteAsync(user);
            return RedirectToAction("Registration", "Home", null);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Edit(string userId)
        {
            UserProfile userProfile = await _webApiCalls.GetUserAsync(userId);
            return View(userProfile);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Edit(string userId, UserProfile profile)
        {
            if (!ModelState.IsValid) return View(profile.UserId);

            var result = await _webApiCalls.UpdateUserAsync(profile.UserId, profile);

            return View("Index", profile);
        }

        [HttpGet("{userId}")]
        public IActionResult ChangePassword(string userId)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.UserId = userId;
           
            return View(model);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> ChangePassword(string userId, ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid || model.NewPassword != model.ConfirmPassword) return View(model.UserId);

            User user = await UserManager.FindByIdAsync(userId);
            var result = await UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if(result.Succeeded)
            {
                UserProfile userProfile = await _webApiCalls.GetUserAsync(userId);
                return View("Index", userProfile);
            }
            return View();
            
        }
    }
}