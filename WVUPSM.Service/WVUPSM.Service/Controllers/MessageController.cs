using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;

namespace WVUPSM.Service.Controllers
{
    /// <summary>
    ///     Controller for Messages
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class MessageController : Controller
    {
        private IMessageRepo _iRepo;

        public MessageController(IMessageRepo iRepo)
        {
            _iRepo = iRepo;
        }

        /// <summary>
        ///     Gets a message
        /// </summary>
        /// <param name="id">Message Id</param>
        /// <returns>The group</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_iRepo.GetMessage(id));
        }

        /// <summary>
        ///     Creates a message from one User to another
        /// </summary>
        /// <returns>The created message</returns>
        [HttpPost]
        public IActionResult Create([FromBody] Message message)
        {
            if (message == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            _iRepo.CreateMessage(message);
            return Created($"api/[controller]/get/{message.Id}", message);  
        }

        /// <summary>
        ///     Gets all messages from database
        /// </summary>
        /// <returns>List of MessageViewModel</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var result = Ok( _iRepo.GetMessages());
            return result;
        }

        /// <summary>
        ///     Gets all messages from one user
        /// </summary>
        /// <returns>List of MessageViewModels</returns>
        [HttpGet("{userId}")]
        public IActionResult Inbox(string userId, int skip = 0, int take = 20)
        {
            var result = Ok( _iRepo.GetInbox(userId, skip, take));
            return result;
        }

        /// <summary>
        ///     Gets all conversation between two users
        /// </summary>
        /// <returns>List of MessageViewModels</returns>
        [HttpGet("{senderId}/{receiverId}")]
        public IActionResult Conversation(string senderId, string receiverId, int skip = 0, int take = 20)
        {
            var result = Ok(_iRepo.GetConversation(senderId, receiverId));
            return result;
        }
    }
}