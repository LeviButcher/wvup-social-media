using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.Models;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
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


        /// <summary>
        ///     User Index Page
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// <param name="tab">From URL Query, data on page depends on which tab is clicked in View 
        /// Tabs are: Profile, Posts, Groups, Following, Followers</param>
        [HttpGet("{userId}")]
        public async Task<IActionResult> Index(string userId, [FromQuery] string tab)
        {
            var user = await _webApiCalls.GetUserAsync(userId);
            ViewData["tab"] = tab ?? "";
            return View(user);
        }



        /// <summary>
        ///     Displays all of a user's followers
        /// </summary>
        /// <param name="userId">user's Id</param>
        [HttpGet("{userId}")]
        public async Task<IActionResult> Followers(string userId)
        {
            ViewData["Title"] = "Followers";
            var followers = await _webApiCalls.GetFollowersAsync(userId);
            return View("UserList", followers);
        }

        /// <summary>
        ///    Display page with all users Following
        /// </summary>
        /// <param name="userId">user's Id</param>
        [HttpGet("{userId}")]
        public async Task<IActionResult> Following(string userId)
        {
            ViewData["Title"] = "Following";
            var following = await _webApiCalls.GetFollowingAsync(userId);
            return View("UserList", following);
        }

        /// <summary>
        ///    When clicked, if user is not following the user with the followId, they will begin following, 
        ///    else the user will unfollow other user
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// /// <param name="followId">The userId of the person being followed/unfollowed</param>
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

        /// <summary>
        ///    Called to check if a follow relationship exists between two users
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// /// <param name="followId">The userId of other user</param>
        [HttpGet("{userId}/{followId}")]
        public async Task<IActionResult> IsFollowing(string userId, string followId)
        {
            var result = await _webApiCalls.IsFollowingAsync(userId, followId);
            return Ok(result);
        }

        /// <summary>
        ///    Displays a view asking the user to confirm if they want to delete their account
        /// </summary>
        /// <param name="userId">user's Id</param>
        [HttpGet("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            UserProfile userProfile = await _webApiCalls.GetUserAsync(userId);
            return View(userProfile);
        }

        /// <summary>
        ///   After confirmation page, if confirmDelete is false, user will be returned to their Index page
        ///   else, User account will be deleted
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// /// <param name="confirmDelete">boolean set by the user clicking to confirm or cancel profile deletion</param>
        /// /// <param name="userProfile">The userId of the person being followed/unfollowed</param>
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

        /// <summary>
        ///  View allows user to edit their UserProfile
        /// </summary>
        /// <param name="userId">user's Id</param>
        [HttpGet("{userId}")]
        public async Task<IActionResult> Edit(string userId)
        {
            UserProfile userProfile = await _webApiCalls.GetUserAsync(userId);
            return View(userProfile);
        }

        /// <summary>
        ///  If there is issue making changes to profile, returns edit View and allows user to resubmit the form
        ///  else, profile is updated and User is redirected to Index page
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// /// <param name="profile">UserProfile ViewModel </param>
        [HttpPost("{userId}")]
        public async Task<IActionResult> Edit(string userId, UserProfile profile)
        {
            if (!ModelState.IsValid) return View(profile);
            var name = profile.UserName == null ? "" : profile.UserName.Trim();
            if (name.Length == 0)
            {
                ModelState.AddModelError("UserName", "User name cannot be empty");
                return View(profile);
            }

            profile.UserName = name;
            var result = await _webApiCalls.UpdateUserAsync(profile.UserId, profile);
            TempData["Announcement"] = "Succesfully updated Profile";
            return RedirectToAction("Index", new {userId});
        }

        /// <summary>
        ///  View allows user to change their password
        /// </summary>
        /// <param name="userId">user's Id</param>
        [HttpGet("{userId}")]
        public IActionResult ChangePassword(string userId)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel
            {
                UserId = userId
            };

            return View(model);
        }

        /// <summary>
        ///  If there is issue changing password, returns ChangePasswordView and allows user to resubmit the form
        ///  else, password is updated and User is redirected to Index page
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// /// <param name="model">ChangePasswordViewModel</param>
        [HttpPost("{userId}")]
        public async Task<IActionResult> ChangePassword(string userId, ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid || model.NewPassword != model.ConfirmPassword) return View(model);

            User user = await UserManager.FindByIdAsync(userId);
            var result = await UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if(result.Succeeded)
            {
                UserProfile userProfile = await _webApiCalls.GetUserAsync(userId);
                return View("Index", userProfile);
            }
            return View();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFollowers(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(await _webApiCalls.GetFollowersAsync(userId, skip, take));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFollowing(string userId,[FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(await _webApiCalls.GetFollowingAsync(userId, skip, take));
        }
    }
}