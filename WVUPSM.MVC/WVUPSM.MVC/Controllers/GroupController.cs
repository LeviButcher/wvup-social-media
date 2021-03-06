﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.WebServiceAccess;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IWebApiCalls _webApiCalls;
        public UserManager<User> UserManager { get; }

        public GroupController(IWebApiCalls webApiCalls, UserManager<User> userManager)
        {
            _webApiCalls = webApiCalls;
            UserManager = userManager;
        }

        /// <summary>
        ///  Group Index page
        /// </summary>
        /// <param name="groupId">Group's Id</param>
        [HttpGet("{groupId}")]
        public async Task<IActionResult> Index(int groupId, [FromQuery] string tab)
        {
            User user = await UserManager.GetUserAsync(HttpContext.User);
            ViewData["tab"] = tab ?? "";
            ViewBag.UserId = user.Id;
            ViewBag.Posts = await _webApiCalls.GetGroupPostsAsync(groupId); 
            var group = await _webApiCalls.GetGroupAsync(groupId);
            return View(group);
        }

        /// <summary>
        ///  A UserList of all members in group with matching groupId
        /// </summary>
        /// <param name="groupId">Group's Id</param>
        [HttpGet]
        public async Task<IActionResult> Members(int groupId)
        {
            var users = await _webApiCalls.GetGroupMembersAsync(groupId);
            ViewData["Title"] = $"Members";
            return View("UserList", users);
        }

        /// <summary>
        ///  Form to create new Group
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await UserManager.GetUserAsync(User);
            GroupViewModel model = new GroupViewModel()
            {
                OwnerId = user.Id,
                OwnerUserName = user.UserName
            };

            return View(model);
        }

        /// <summary>
        ///  HttpPost to Create a Group
        ///  if Model is invalid, redisplays form
        ///  Otherwise, group is created and redirects to Group's Index page
        /// </summary>
        /// <param name="model">GroupViewModel </param>
        [HttpPost]
        public async Task<IActionResult> Create(GroupViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            Group newGroup = new Group()
            {
                OwnerId = model.OwnerId,
                Bio = model.Bio,
                Name = model.GroupName
            };

            var result = await _webApiCalls.CreateGroupAsync(newGroup);
            var resultGroup = JsonConvert.DeserializeObject<Group>(result);
            if (resultGroup == null) return View(model);

            return RedirectToAction("Index", "Group", new { groupId = resultGroup.Id});
        }

        /// <summary>
        ///  Uses groupId to create a GroupViewModel
        /// </summary>
        /// <param name="groupId">Group's Id</param>
        [HttpGet("{groupId}")]
        public async Task<IActionResult> Delete(int groupId)
        {
            GroupViewModel groupViewModel = await _webApiCalls.GetGroupAsync(groupId);
            return View(groupViewModel);
        }

        /// <summary>
        ///  If confirmDelete is false, redirect to Group Index Page
        ///  Else, retreive groupOwner, and All posts in group
        ///  All Group Posts must be deleted, and then group may be deleted
        ///  After successful deletion, redirect's to groupOwner's Index page
        /// </summary>
        /// <param name="groupId">Group's Id</param>
        [HttpPost("{groupId}/{confirmDelete}")]
        public async Task<IActionResult> Delete(int groupId, bool confirmDelete, GroupViewModel groupViewModel)
        {
            if (!confirmDelete)
            {
                return RedirectToAction("Index");
            }

            var user = await _webApiCalls.GetGroupOwner(groupId);
            var posts = await _webApiCalls.GetGroupPostsAsync(groupId);

            foreach(UserPost post in posts)
            {
                await _webApiCalls.DeletePostAsync(post.PostId);
            }

            await _webApiCalls.DeleteGroupAsync(groupId);

            return RedirectToAction("Index", "User", new { userId = user.UserId });
        }

        /// <summary>
        ///  Returns a list of all groups the logged in User is a member of
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> MyGroups()
        {
            User user = await UserManager.GetUserAsync(HttpContext.User);
            TempData["UserId"] = user.Id;
            

            return View();
        }

        /// <summary>
        /// Returns json of a user's groups
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> Users(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return PartialView("_GroupList", await _webApiCalls.GetUsersGroupsAsync(userId, skip, take));
        }

        /// <summary>
        ///      Returns json of group's members
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>JSON</returns>
        [HttpGet("{groupId}")]
        public async Task<IActionResult> Members(int groupId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return PartialView("_UserProfileList", await _webApiCalls.GetGroupMembersAsync(groupId, skip, take));
        }

        /// <summary>
        ///     returns json of group's posts
        /// </summary>
        /// <param name="groupId">Id of group</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>JSON</returns>
        [HttpGet("{groupId}")]
        public async Task<IActionResult> Posts(int groupId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return PartialView("_UserPostList", await _webApiCalls.GetGroupPostsAsync(groupId, skip, take));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/{groupId}")]
        public async Task<IActionResult> IsMember(string userId, int groupId)
        {
            return Ok(await _webApiCalls.IsMember(userId, groupId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost("{userId}/{groupId}")]
        public async Task<IActionResult> ToggleJoin(string userId, int groupId)
        {
            var isMember = await _webApiCalls.IsMember(userId, groupId);

            if (isMember)
            {
                await _webApiCalls.LeaveGroupAsync(userId, groupId);
                return NoContent();
            }
            else
            {
                await _webApiCalls.JoinGroupAsync(userId, groupId);
                return Ok();
            }
        }
    }
}
