using Microsoft.AspNetCore.Hosting;
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

        [HttpGet("{userId}")]
        public IActionResult Following(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(_pRepo.GetFollowingPosts(userId, skip, take));
        }

        //All of the Users post
        [HttpGet("{userId}")]
        public IActionResult Me(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(_pRepo.GetUsersPost(userId, skip, take));
        }

        [HttpPost]
        public IActionResult Create(CreatePost post)
        {
            if (post == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            Post basePost = new Post
            {
                Text = post.Text,
                UserId = post.UserId,
            };

            if(post.File != null)
            {
                var uniqueFileName = GetUniqueFileName(post.File.FileName);
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                post.File.CopyTo(new FileStream(filePath, FileMode.Create));

                //Set the fileName for the basePost
                basePost.PicturePath = uniqueFileName;
            }

            //TODO: Add a deletion for when a post fails to create if a file was uploaded

            _pRepo.CreatePost(basePost);
            return Created($"api/post/get/{basePost.Id}", post);
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

        [HttpDelete("{postId}")]
        public IActionResult Delete(int postId)
        {
            var item = _pRepo.GetBasePost(postId);

            if (item == null) return NotFound();

            _pRepo.DeletePost(item);
            return NoContent();
        }
    }
}
