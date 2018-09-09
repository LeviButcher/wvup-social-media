using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public PostController(IPostRepo pRepo)
        {
            _pRepo = pRepo;
        }

        [HttpGet]
        public IActionResult Get(int postId)
        {
            var item = _pRepo.GetPost(postId);
            if(item == null)
            {
                return NotFound();
            }
            return Json(item);
        }

        [HttpGet]
        public IActionResult Get(int skip = 0, int take = 10)
        {
            return NotFound();
        }

        [HttpGet]
        public IActionResult Following(string userId, UserProfile user, int skip = 0, int take = 10)
        {
            if (userId != user.UserId) return NotFound();

            return Ok(_pRepo.GetFollowPosts(userId, skip, take));
        }

        //All of the Users post
        public IActionResult Me(string userId, UserProfile user, int skip = 0, int take = 10)
        {
            if (userId != user.UserId) return NotFound();

            return Ok(_pRepo.GetUsersPost(userId, skip, take));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Post post)
        {
            if (post == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            _pRepo.CreatePost(post);
            return Created($"api/[controller]/get/{post.Id}", post);
        }

        [HttpPut]
        public IActionResult Update(int postId, [FromBody] Post post)
        {
            if (post == null || postId != post.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            //Add in update code
            return Accepted();
        }

        [HttpDelete]
        public IActionResult Delete(int postId, [FromBody] Post post)
        {
            if (post == null || postId != post.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            _pRepo.DeletePost(post);
            return NoContent();
        }
    }
}
