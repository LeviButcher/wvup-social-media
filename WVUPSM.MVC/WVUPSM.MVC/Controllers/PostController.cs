using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class PostController : Controller
    {
        public IWebApiCalls WebApiCalls { get; }
        public UserManager<User> UserManager { get; }
        public FileController FileController { get; }

        private IHostingEnvironment _env;

        public PostController(IWebApiCalls webApiCalls, UserManager<User> userManager, IHostingEnvironment env, FileController fileController)
        {
            WebApiCalls = webApiCalls;
            UserManager = userManager;
            _env = env;
            FileController = fileController;
        }

        /// <summary>
        ///     Post Index Page 
        ///     Displays post content
        /// </summary>
        /// <param name="postId">Id of post</param>
        [HttpGet("{postId}")]
        public async Task<IActionResult> Index(int postId)
        {
            User user = await UserManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user.Id;
            var post = await WebApiCalls.GetPostAsync(postId);
            var comments = await WebApiCalls.GetCommentsAsync(post.PostId);
            ViewBag.Comments = comments;
            return View(post);
        }

        /// <summary>
        ///     Gets post from one user
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// <param name="skip">From URL Query, used for paging, default is 0
        /// <param name="take">From URL Query, used for paging, default is 10
        /// /// Tabs are: Profile, Posts, Groups, Following, Followers</param>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPosts(string userId, [FromQuery] int skip, [FromQuery] int take)
        {
            var posts = await WebApiCalls.GetMyPostAsync(userId, skip, take);
            return Ok(posts);
        }

        /// <summary>
        ///    Delete post - after deletion, redirects to Home controller's Index page
        /// </summary>
        /// <param name="postId">post's Id</param>
        [HttpPost("{postId}")]
        public async Task<IActionResult> Delete(int postId)
        {
            var post = await WebApiCalls.GetPostAsync(postId);
            //if (post.FilePath != null)
            //{
            //    var dirPath = Path.Combine(_env.WebRootPath, "uploads", post.FilePath);
            //    System.IO.File.Delete(dirPath);
            //}
            await WebApiCalls.DeletePostAsync(postId);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        ///    Create Post
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await UserManager.GetUserAsync(User);

            ViewBag.Groups = await WebApiCalls.GetGroupsForDropdown(user.Id);
            
            CreatePost model = new CreatePost()
            {
                UserName = user.UserName,
                UserId = user.Id
            };

            return View(model);
        }

        /// <summary>
        ///     Sends the Comment passed in to the service layer and saves it
        /// </summary>
        /// <param name="comment">Comment to save</param>
        /// <returns>View of the post commented on</returns>
        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", new { postId = comment.PostId });
            var result = await WebApiCalls.CreateCommentAsync(comment);
            if(result != null)
            {
                return RedirectToAction("Index", new { postId = comment.PostId });
            }
            return RedirectToAction("Index", new { postId = comment.PostId });
        }

        /// <summary>
        ///    Create post
        ///    If model is invalid, return to view so user can submit form
        ///    If the both is empty, return to view so user can submit form
        ///    If post's groupId is -1, this means it is a post on the user's own page,
        ///    else post is a group post
        ///    If post has a file attached, file is copied to fileStream and stream is closed
        /// </summary>
        /// <param name="post">CreatePost viewModel</param>
        [HttpPost]
        public async Task<IActionResult> Create(CreatePost post)
        {
            if (!ModelState.IsValid) return View(post);

            if(post.File == null && post.Text == null)
            {
                return View(post);
            }

            Post basePost = new Post();

            if (post.GroupId != -1)
            {
                basePost.Text = post.Text;
                basePost.GroupId = post.GroupId;
                basePost.UserId = post.UserId;
            }
            else
            {
                basePost.Text = post.Text;
                basePost.UserId = post.UserId;
            }

            if(post.File != null)
            {
                //Start upload of file
                var fileResult = await FileController.Create(post.File);
                if (fileResult > 0)
                {
                    basePost.FileId = fileResult;
                }
            }
            

            var result = await WebApiCalls.CreatePostAsync(basePost);
            var resultUser = JsonConvert.DeserializeObject<Post>(result);
            if (resultUser == null) return View(post);

            return RedirectToAction("Index", "User", new { userId = resultUser.UserId, tab = "Posts"});
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
