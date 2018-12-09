using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.WebServiceAccess.Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVUPSM.MVC.Controllers
{
    /// <summary>
    ///     Message Controller for Message actions and views
    /// </summary>
    [Route("[controller]/[action]")]
    [Authorize]
    public class MessageController : Controller
    {
        public IWebApiCalls WebApiCalls { get; }
        public UserManager<User> UserManager { get; }
        private IHostingEnvironment _env;

        public MessageController(IWebApiCalls webApiCalls, UserManager<User> userManager, IHostingEnvironment env)
        {
            WebApiCalls = webApiCalls;
            UserManager = userManager;
            _env = env;
        }

        /// <summary>
        ///     Inbox view for the current user
        /// </summary>
        /// <returns></returns>
        ///
        [HttpGet]
        public async Task<IActionResult> Inbox()
        {
            User currentUser = await UserManager.GetUserAsync(HttpContext.User);
            var messages = await WebApiCalls.GetInboxAsync(currentUser.Id);
            return View(messages);
        }

        /// <summary>
        /// Gets the currently logged in users conversation messages 
        /// between a user
        /// </summary>
        /// <param name="otherUserId">id of another user</param>
        /// <param name="skip">records to skip</param>
        /// <param name="take">records to take</param>
        /// <returns>MessageViewModels of the messages</returns>
        [HttpGet("{otherUserId}")]
        public async Task<IActionResult> Conversation(string otherUserId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var currUserId = UserManager.GetUserId(User);
            return PartialView("~/Views/Message/_MessageList.cshtml", await WebApiCalls.GetConversationAsync(currUserId, otherUserId, skip, take));
        }

        /// <summary>
        ///     Gets the Messsage View to send a message to the user whose ID matches the one provided
        /// </summary>
        /// <param name="userId">User ID to message</param>
        /// <returns>View populated with messages to this user</returns>
        [Route("~/[controller]/{userId}")]
        public async Task<IActionResult> Message(string userId)
        {
            //Created a base Message here for Form
            //Get the current user
            //built out message  model senderId to current user
            //build out messsage recieverId to userId
            User currentUser = await UserManager.GetUserAsync(HttpContext.User);
            var otherUser = await WebApiCalls.GetUserAsync(userId);
            Message model = new Message()
            {
                SenderId = currentUser.Id,
                ReceiverId = userId,
            };

            var messages = await WebApiCalls.GetConversationAsync(currentUser.Id, userId);
            ViewBag.Messages = messages;
            ViewBag.UserId = currentUser.Id;
            //For post scrolling
            ViewData["otherUser"] = userId;
            ViewData["currUser"] = currentUser.Id;
            ViewData["OtherUserName"] = otherUser.UserName;
            return View(model);
        }

        /// <summary>
        ///     Message Post action sending a Message to the Database
        /// </summary>
        /// <param name="message">Message to save</param>
        /// <returns>Redirect to action for Message</returns>
        [HttpPost]
        public async Task<IActionResult> Message(Message message)
        {
            if (!ModelState.IsValid) return View(message);

            var result = await WebApiCalls.CreateMessageAsync(message);

            return RedirectToAction($"{message.ReceiverId}");
        }
    }
}
