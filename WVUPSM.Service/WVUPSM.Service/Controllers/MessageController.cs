﻿using System;
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
        private INotificationRepo _nRepo { get; }

        public MessageController(IMessageRepo iRepo, INotificationRepo nRepo)
        {
            _iRepo = iRepo;
            _nRepo = nRepo;
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
            _nRepo.CreateNotification(new Notification() {UserId = message.ReceiverId, InteractingUserId = message.SenderId });

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
        public IActionResult Inbox(string userId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var result = Ok( _iRepo.GetInbox(userId, skip, take));
            return result;
        }

        /// <summary>
        ///     Gets all conversation between two users
        /// </summary>
        /// <returns>List of MessageViewModels</returns>
        [HttpGet("{senderId}/{receiverId}")]
        public IActionResult Conversation(string senderId, string receiverId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var result = Ok(_iRepo.GetConversation(senderId, receiverId, skip, take));
            return result;
        }

        /// <summary>
        ///     Gets paging view model for inbox
        /// </summary>
        /// <returns>List of MessageViewModels</returns>
        [HttpGet("{userId}")]
        public IActionResult InboxDetails(string userId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 1)
        {
            return Ok(_iRepo.GetInboxDetails(userId, pageSize, pageIndex));
        }
    }
}
