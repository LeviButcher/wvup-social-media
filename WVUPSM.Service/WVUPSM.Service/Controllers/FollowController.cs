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
    /// <summary>
    ///     Controller for Follows
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class FollowController : Controller
    {
        private IFollowRepo _iRepo;

        public FollowController(IFollowRepo iRepo)
        {
            _iRepo = iRepo;
        }

        /// <summary>
        ///     Gets if a user is following another user
        /// </summary>
        /// <returns>User's ID and the follower's ID</returns>
        [HttpGet("{userId}/{followId}")]
        public async Task<IActionResult> IsFollowing(string userId, string followId)
        {
            return Ok(await _iRepo.IsFollowingAsync(userId, followId));
        }

        /// <summary>
        ///     Gets a list of followers
        /// </summary>
        /// <returns>The list of followers</returns>
        [HttpGet("{userId}")]
        public IActionResult Followers(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var result = Ok(_iRepo.GetFollowers(userId, skip, take));
            return result;
        }

        /// <summary>
        ///     Gets a list of who you are following
        /// </summary>
        /// <returns>The a list of who you are following</returns>
        [HttpGet("{userId}")]
        public IActionResult Following(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var result = Ok(_iRepo.GetFollowing(userId, skip, take));
            return result;
        }

        /// <summary>
        ///     Creates a follow
        /// </summary>
        /// <returns>That you followed someone</returns>
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

        /// <summary>
        ///     Removes that you are following someone
        /// </summary>
        /// <returns>Removes the person from your following list</returns>
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

        /// <summary>
        ///     Gets a count of how many follow you
        /// </summary>
        /// <returns>The count of how many follow you</returns>
        [HttpGet("{userId}")]
        public IActionResult FollowerCount(string userId)
        {
            return Ok(_iRepo.GetFollowerCount(userId));
        }

        /// <summary>
        ///     Gets a count of how many you follow
        /// </summary>
        /// <returns>The count of how many you follow</returns>
        [HttpGet("{userId}")]
        public IActionResult FollowingCount(string userId)
        {
            return Ok(_iRepo.GetFollowingCount(userId));
        }
    }
}
