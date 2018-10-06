using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;


namespace WVUPSM.Service.Controllers
{
    /// <summary>
    ///     Controller for Posts
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class PostController : Controller
    {
        private IPostRepo _pRepo;
        private IHostingEnvironment _env;

        public PostController(IPostRepo pRepo, IHostingEnvironment env)
        {
            _pRepo = pRepo;
            _env = env;
        }

        /// <summary>
        ///     Gets a post
        /// </summary>
        /// <returns>The post</returns>
        [HttpGet("{postId}")]
        public IActionResult Get(int postId)
        {
            var item = _pRepo.GetPost(postId);
            if(item == null)
            {
                return NotFound();
            }
            return Json(item);
        }

        // Not implemented
        [HttpGet]
        public IActionResult Get([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return NotFound();
        }

        /// <summary>
        ///     Gets all posts from users the user is following
        /// </summary>
        /// <returns>All followed user's posts</returns>
        [HttpGet("{userId}")]
        public IActionResult Following(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(_pRepo.GetFollowingPosts(userId, skip, take));
        }

        /// <summary>
        ///     Gets all of a user's posts
        /// </summary>
        /// <returns>All of the user's posts</returns>
        [HttpGet("{userId}")]
        public IActionResult Me(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(_pRepo.GetUsersPost(userId, skip, take));
        }

        /// <summary>
        ///     Allows a user to create a post
        /// </summary>
        /// <returns>The created post</returns>
        [HttpPost]
        public IActionResult Create([FromBody] Post post) 
        {
            if (post == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            if(post.Text == null && post.FilePath == null)
            {
                return BadRequest();
            }

            _pRepo.CreatePost(post);
            return Created($"api/post/get/{post.Id}", post);
        }
        
        /// <summary>
        ///     Allows a user to update a post
        /// </summary>
        /// <returns>The updated post</returns>
        [HttpPut("{postId}")]
        public IActionResult Update(int postId, [FromBody] Post post)
        {
            if (post == null || postId != post.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            //Add in update code
            return Accepted();
        }

        /// <summary>
        ///     Deletes a post
        /// </summary>
        /// <returns>That the post has been deleted</returns>
        [HttpDelete("{postId}")]
        public IActionResult Delete(int postId)
        {
            var item = _pRepo.GetBasePost(postId);

            if (item == null) return NotFound();

            _pRepo.DeletePost(item);
            return NoContent();
        }

        /// <summary>
        ///     Gets all group posts
        /// </summary>
        /// <returns>All group posts</returns>
        [HttpGet("{groupId}")]
        public IActionResult GroupPosts(int groupId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(_pRepo.GetGroupPost(groupId, skip, take));
        }
    }
}
