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
    ///     Controller for Groups
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class GroupController : Controller
    {
        private IGroupRepo _iRepo;

        public GroupController(IGroupRepo iRepo)
        {
            _iRepo = iRepo;
        }

        /// <summary>
        ///     Gets all group members
        /// </summary>
        /// <returns>All group members</returns>
        [HttpGet("{groupId}")]
        public IActionResult Members(int groupId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var result = Ok( _iRepo.GetGroupMembers(groupId, skip, take));
            return result;
        }

        /// <summary>
        ///     Gets a list of groups a user is in or owns
        /// </summary>
        /// <returns>The list of groups a user is in or owns</returns>
        [HttpGet("{userId}")]
        public IActionResult UsersGroups(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var result = Ok( _iRepo.GetUsersGroups(userId, skip, take));
            return result;
        }

        /// <summary>
        ///     Gets a count of all group members
        /// </summary>
        /// <returns>The count of all group members</returns>
        [HttpGet("{groupId}")]
        public IActionResult MemberCount(int groupId)
        {
            var result = Ok(_iRepo.GetMemberCount(groupId));
            return result;
        }

        /// <summary>
        ///     Gets the group owner's name
        /// </summary>
        /// <returns>The group owner's name</returns>
        [HttpGet("{id}")]
        public IActionResult GetOwner(int id)
        {
            return Ok(_iRepo.GetOwner(id));
        }

        /// <summary>
        ///     Gets a group
        /// </summary>
        /// <returns>The group</returns>
        [HttpGet("{id}")]
        public IActionResult GetGroup(int id)
        {
            return Ok(_iRepo.GetGroup(id));
        }

        /// <summary>
        ///     Gets the base group
        /// </summary>
        /// <returns>The base group</returns>
        [HttpGet("{id}")]
        public IActionResult GetBaseGroup(int id)
        {
            return Ok(_iRepo.GetBaseGroup(id));
        }
        
        /// <summary>
        ///     Sees if a user is the group owner
        /// </summary>
        /// <returns>If the user is the group owner or not</returns>
        [HttpGet("{userId}/{groupId}")]
        public async Task<IActionResult> IsOwner(string userId, int groupId)
        {
            var result = await _iRepo.IsOwner(userId, groupId);
            return Ok(result);
            
        }

        /// <summary>
        ///     Takes a perameter term to look for groups
        /// </summary>
        /// <returns>A list of groups that match the term used</returns>
        [HttpGet("{term}")]
        public IActionResult Search(string term)
        {
            var users = _iRepo.FindGroups(term);
            return Ok(users);
        }

        /// <summary>
        ///     Gets if a user is a member of a group
        /// </summary>
        /// <returns>If the user is a member of the group or not</returns>
        [HttpGet("{userId}/{groupId}")]
        public async Task<IActionResult> IsMember(string userId, int groupId)
        {
            return Ok(await _iRepo.IsMember(userId, groupId));
        }

        /// <summary>
        ///     Lets you join a group
        /// </summary>
        /// <returns>If you have joined or not</returns>
        [HttpGet("{userId}/{groupId}")]
        public async Task<IActionResult> Join(string userId, int groupId)
        {
            var result = Ok(await _iRepo.JoinGroup(userId, groupId));
            return result;
        }

        /// <summary>
        ///     Lets you leave a group
        /// </summary>
        /// <returns>If you have left the group or not</returns>
        [HttpGet("{userId}/{groupId}")]
        public async Task<IActionResult> Leave(string userId, int groupId)
        {
            var result = Ok(await _iRepo.LeaveGroup(userId, groupId));
            return result;
        }

        /// <summary>
        ///     Lets a user make a group
        /// </summary>
        /// <returns>The created group</returns>
        [HttpPost]
        public IActionResult Create([FromBody] Group group)
        {
            if (group == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            _iRepo.CreateGroup(group);
            return Created($"api/[controller]/get/{group.Id}", group);
        }

        /// <summary>
        ///     Allows the owner of a group to update information about it
        /// </summary>
        /// <returns>The updated information</returns>
        [HttpPut("{groupId}")]
        public IActionResult Update(int groupId, [FromBody] Group group)
        {
            if (group == null || groupId != group.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            //Add in update code
            return Accepted();
        }

        /// <summary>
        ///     Allows the owner to remove the group
        /// </summary>
        /// <returns>That the group has been deleted</returns>
        [HttpDelete("{groupId}")]
        public IActionResult Delete(int groupId)
        {
            var item = _iRepo.GetBaseGroup(groupId);

            if (item == null) return NotFound();

            _iRepo.DeleteGroup(item);
            return NoContent();
        }
    }
}
