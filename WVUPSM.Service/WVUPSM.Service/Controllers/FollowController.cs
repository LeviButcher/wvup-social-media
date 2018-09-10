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
    public class FollowController : Controller
    {
        private IFollowRepo _iRepo;

        public FollowController(IFollowRepo iRepo)
        {
            _iRepo = iRepo;
        }

        [HttpGet("{userId}")]
        public IActionResult Followers(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(_iRepo.GetFollowers(userId, skip, take));
        }

        [HttpGet("{userId}")]
        public IActionResult Following(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(_iRepo.GetFollowing(userId, skip, take));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Follow follow)
        {
            if (follow == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            _iRepo.CreateFollower(follow);
            return Created($"api/[controller]/get/{follow.FollowId}", follow);
        }

        [HttpDelete("{userId}")]
        public IActionResult Delete(string userId, [FromBody] Follow follow)
        {
            if (follow == null || userId != follow.UserId || !ModelState.IsValid)
            {
                return BadRequest();
            }

            _iRepo.DeleteFollower(follow);
            return NoContent();
        }

        [HttpGet("{userId}")]
        public IActionResult FollowerCount(string userId)
        {
            return Ok(_iRepo.GetFollowerCount(userId));
        }

        [HttpGet("{userId}")]
        public IActionResult FollowingCount(string userId)
        {
            return Ok(_iRepo.GetFollowingCount(userId));
        }
    }
}
