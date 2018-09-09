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
    [Route("api/[controller]")]
    public class FollowController : Controller
    {
        private IFollowRepo _iRepo;

        public FollowController(IFollowRepo iRepo)
        {
            _iRepo = iRepo;
        }

        [HttpGet]
        public IActionResult Followers(string userId, UserProfile user, int skip = 0, int take = 10)
        {
            if (userId != user.UserId) return NotFound();

            return Ok(_iRepo.GetFollowers(userId, skip, take));
        }

        [HttpGet]
        public IActionResult Following(string userId, UserProfile user, int skip = 0, int take = 10)
        {
            if (userId != user.UserId) return NotFound();

            return Ok(_iRepo.GetFollowing(userId, skip, take));
        }

        [HttpPost]
        public IActionResult Create(Follow follow)
        {
            if (follow == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            _iRepo.CreateFollower(follow);
            return Created($"api/[controller]/get/{follow.FollowId}", follow);
        }

        [HttpDelete]
        public IActionResult Delete(string userId, Follow follow)
        {
            if (follow == null || userId != follow.UserId || !ModelState.IsValid)
            {
                return BadRequest();
            }

            _iRepo.DeleteFollower(follow);
            return NoContent();
        }

        [HttpGet]
        public IActionResult FollowerCount(string userId, UserProfile user)
        {
            if (user.UserId != userId) return NotFound();

            return Ok(_iRepo.GetFollowerCount(userId));
        }

        [HttpGet]
        public IActionResult FollowingCount(string userId, UserProfile user)
        {
            if (user.UserId != userId) return NotFound();

            return Ok(_iRepo.GetFollowingCount(userId));
        }
    }
}
