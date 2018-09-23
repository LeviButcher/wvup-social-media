using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private IHostingEnvironment _env;

        public PostController(IWebApiCalls webApiCalls, UserManager<User> userManager, IHostingEnvironment env)
        {
            WebApiCalls = webApiCalls;
            UserManager = userManager;
            _env = env;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> Index(int postId)
        {
            User user = await UserManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user.Id;
            var post = await WebApiCalls.GetPostAsync(postId);
            return View(post);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPosts(string userId, [FromQuery] int skip, [FromQuery] int take)
        {
            var posts = await WebApiCalls.GetMyPostAsync(userId, skip, take);
            return Ok(posts);
        }

        [HttpPost("{postId}")]
        public async Task<IActionResult> Delete(int postId)
        {
            var post = await WebApiCalls.GetPostAsync(postId);
            if (post.PicturePath != null)
            {
                var dirPath = Path.Combine(_env.WebRootPath, "uploads", post.PicturePath);
                System.IO.File.Delete(dirPath);
            }
            await WebApiCalls.DeletePostAsync(postId);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await UserManager.GetUserAsync(User);
            CreatePost model = new CreatePost()
            {
                UserName = user.UserName,
                UserId = user.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePost post)
        {
            if (!ModelState.IsValid) return View(post);

            Post basePost = new Post()
            {
                Text = post.Text,
                UserId = post.UserId
            };

            if (post.File!= null)
            {
                var uniqueFileName = GetUniqueFileName(post.File.FileName);
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                post.File.CopyTo(fileStream);
                //Ensure that the stream is closed
                fileStream.Close();

                //Set the fileName for the basePost
                basePost.PicturePath = uniqueFileName;
            }

            var result = await WebApiCalls.CreatePostAsync(basePost);
            var resultUser = JsonConvert.DeserializeObject<Post>(result);
            if (resultUser == null) return View(post);

            return RedirectToAction("Index", "User", new { userId = resultUser.UserId});
        }

        /// <summary>
        ///     Generate a unique file name for storing files underneath the wwwroot folder
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Unique file name</returns>
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
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
