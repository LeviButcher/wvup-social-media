using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;

namespace WVUPSM.Service.Controllers
{
    /// <summary>
    ///     Controller for Tags
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class TagController : Controller
    {
        private ITagRepo _iRepo;

        public TagController(ITagRepo iRepo)
        {
            _iRepo = iRepo;
        }

        /// <summary>
        ///     Creates a new Tag
        /// </summary>
        /// <returns>The created group</returns>
        [HttpPost("{userId}")]
        public IActionResult Create(string userId, [FromBody] Tag tag)
        {
            if (tag == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            string[] tags = tag.Name.Split(' ');
            foreach( var word in tags)
            {
                _iRepo.CreateTag(word, userId);
            }
            //var result = _iRepo.CreateTag(tag, userId);
            // IEnumerable<UserTag> userTags = _iRepo.GetUserTagsByUser(userId);
            return RedirectToAction($"api/[controller]/GetUsers/{userId}");

            //if (result > 0)
            //{
            //    return Created($"api/[controller]/get/{tag.Id}", tag);
            //}
            //else
            //{
            //    return RedirectToAction($"api/[controller]/get/{tag.Id}", tag);
            //}
        }

        [HttpPost("{tagId}/{userId}")]
        public IActionResult Remove(int tagId, string userId)
        {

            var item = new UserTag()
            {
                TagId = tagId,
                UserId = userId
            };

            _iRepo.RemoveUserTag(tagId, userId);

            return NoContent();
        }

        [HttpGet("{userid}")]
        public IActionResult GetUsers(string userId)
        {
             return Ok(_iRepo.GetUserTagsByUser(userId));
        }

        [HttpGet("{tagId}")]
        public IActionResult GetTags(int tagId)
        {
            return Ok(_iRepo.GetUserTagsByTag(tagId));
        }

        
    }
}
