using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;

namespace WVUPSM.Service.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GroupController : Controller
    {
        private IGroupRepo _iRepo;

        public GroupController(IGroupRepo iRepo)
        {
            _iRepo = iRepo;
        }

        [HttpGet("{groupId}")]
        public IActionResult Members(int groupId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var result = Ok( _iRepo.GetGroupMembers(groupId, skip, take));
            return result;
        }

        [HttpGet("{userId}")]
        public IActionResult UsersGroups(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var result = Ok( _iRepo.GetUsersGroups(userId, skip, take));
            return result;
        }

        [HttpGet("{groupId}")]
        public IActionResult MemberCount(int groupId)
        {
            var result = Ok(_iRepo.GetMemberCount(groupId));
            return result;
        }

        [HttpGet("{id}")]
        public IActionResult GetOwner(int id)
        {
            return Ok(_iRepo.GetOwner(id));
        }

        [HttpGet("{id}")]
        public IActionResult GetGroup(int id)
        {
            return Ok(_iRepo.GetGroup(id));
        }

        [HttpGet("{id}")]
        public IActionResult GetBaseGroup(int id)
        {
            return Ok(_iRepo.GetBaseGroup(id));
        }



        [HttpGet("{userId}/{groupId}")]
        public async Task<IActionResult> IsOwner(string userId, int groupId)
        {
            var result = await _iRepo.IsOwner(userId, groupId);
            return Ok(result);
            
        }

        [HttpGet("{term}")]
        public IActionResult Search(string term)
        {
            var users = _iRepo.FindGroups(term);
            return Ok(users);
        }

        [HttpGet("{userId}/{groupId}")]
        public async Task<IActionResult> IsMember(string userId, int groupId)
        {
            return Ok(await _iRepo.IsMember(userId, groupId));
        }


        [HttpGet("{userId}/{groupId}")]
        public async Task<IActionResult> Join(string userId, int groupId)
        {
            var result = Ok(await _iRepo.JoinGroup(userId, groupId));
            return result;
        }

        [HttpGet("{userId}/{groupId}")]
        public async Task<IActionResult> Leave(string userId, int groupId)
        {
            var result = Ok(await _iRepo.LeaveGroup(userId, groupId));
            return result;
        }

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
