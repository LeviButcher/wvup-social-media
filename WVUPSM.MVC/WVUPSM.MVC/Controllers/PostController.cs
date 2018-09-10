using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    public class PostController : Controller
    {
        public IWebApiCalls WebApiCalls { get; }
        public UserManager<User> UserManager { get; }

        public PostController(IWebApiCalls webApiCalls, UserManager<User> userManager)
        {
            WebApiCalls = webApiCalls;
            UserManager = userManager;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> Index(int postId)
        {
            var post = await WebApiCalls.GetPostAsync(postId);
            return View(post);
        }

        //Don't worry about this one
        public IActionResult Me()
        {
            return View();
        }

        [HttpDelete("{postId}")]
        public IActionResult Delete(int postId, UserPost post)
        {

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await UserManager.GetUserAsync(User);
            UserPost model = new UserPost()
            {
                UserName = user.UserName,
                UserId = user.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserPost post)
        {
            if (!ModelState.IsValid) return View(post);
            Post newPost = new Post()
            {
                UserId = post.UserId,
                Text = post.Text,
            };

            var result = await WebApiCalls.CreatePostAsync(newPost);
            var resultUser = JsonConvert.DeserializeObject<Post>(result);
            if (resultUser == null) return View(post);

            return RedirectToAction("Index", "User", new { userId = resultUser.UserId});
        }

        //Don't worry about edits
        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Edit(int postId, UserPost post)
        {
            return View();
        }
    }
}
