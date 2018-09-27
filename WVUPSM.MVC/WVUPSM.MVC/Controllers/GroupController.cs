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
    public class GroupController : Controller
    {
        private readonly IWebApiCalls _webApiCalls;
        public UserManager<User> UserManager { get; }

        public GroupController(IWebApiCalls webApiCalls, UserManager<User> userManager)
        {
            _webApiCalls = webApiCalls;
            UserManager = userManager;
        }

        [HttpGet("{groupId}")]
        public async Task<IActionResult> Index(int groupId)
        {
            User user = await UserManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user.Id;
            ViewBag.Posts = await _webApiCalls.GetGroupPostsAsync(groupId);
            var group = await _webApiCalls.GetGroupAsync(groupId);
            return View(group);
        }

        [HttpGet]
        public async Task<IActionResult> Members( int groupId)
        {
            var users = await _webApiCalls.GetGroupMembersAsync(groupId);
            ViewData["Title"] = $"Members";
            return View("UserList", users);
        }

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

        [HttpGet("{groupId}")]
        public async Task<IActionResult> Delete(int groupId)
        {
            GroupViewModel groupViewModel = await _webApiCalls.GetGroupAsync(groupId);
            return View(groupViewModel);
        }

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


    }
}
