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
            return Ok(_pRepo.GetFollowersPosts(userId, skip, take));
        }

        //All of the Users post
        [HttpGet("{userId}")]
        public IActionResult Me(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
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
