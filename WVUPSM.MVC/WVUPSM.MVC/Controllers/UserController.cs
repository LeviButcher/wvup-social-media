using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public UserController(IWebApiCalls webApiCalls)
        {
            _webApiCalls = webApiCalls;
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

        //Delete Confirmation Page
        [HttpGet]
        public IActionResult Delete(string userId)
        {
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(string userId, UserProfile user, bool confirmed)
        {
            return View();
        }

        //Update User Profile
        //Don't worry about it
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

            return View("Index");
        }
    }
}