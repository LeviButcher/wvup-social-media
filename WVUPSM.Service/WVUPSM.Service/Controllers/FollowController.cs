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

        [HttpGet("{userId}/{followId}")]
        public async Task<IActionResult> IsFollowing(string userId, string followId)
        {
            return Ok(await _iRepo.IsFollowingAsync(userId, followId));
        }

        [HttpGet("{userId}")]
        public IActionResult Followers(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var result = Ok(_iRepo.GetFollowers(userId, skip, take));
            return result;
        }

        [HttpGet("{userId}")]
        public IActionResult Following(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {

            var result = Ok(_iRepo.GetFollowing(userId, skip, take));
            return result;
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

        [HttpDelete("{userId}/{followId}")]
        public IActionResult Delete(string userId, string followId)
        {
            var item = new Follow()
            {
                UserId = userId,
                FollowId = followId
            };

            _iRepo.DeleteFollower(item);
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
