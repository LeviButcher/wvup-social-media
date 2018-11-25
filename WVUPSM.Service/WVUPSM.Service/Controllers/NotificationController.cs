using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.DAL.Repos.Interfaces;

namespace WVUPSM.Service.Controllers
{
    [Route("api/[controller]/[action]")]
    public class NotificationController : Controller
    {
        private INotificationRepo _nRepo { get; }

        public NotificationController(INotificationRepo nRepo)
        {
            _nRepo = nRepo;
        }

        [HttpGet("{userId}")]
        public ActionResult Today(string userId)
        {
            return Ok(_nRepo.GetTodaysNotifcations(userId));
        }

        [HttpGet("{userId}")]
        public ActionResult Unread(string userId,[FromQuery] int skip = 0,[FromQuery] int take = 10)
        {
            return Ok(_nRepo.GetUsersUnreadNotifications(userId, skip, take));
        }

        /// <summary>
        ///     Method for getting information about paging on Unread records
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>PagingViewModel</returns>
        [HttpGet("~/api/Notification/Unread/Page/{userId}")]
        public ActionResult UnreadPage(string userId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 1)
        {
            return Ok(_nRepo.GetUnreadPageDetails(userId, pageSize, pageIndex));
        }

        [HttpGet("{userId}")]
        public ActionResult Read(string userId, [FromQuery] int skip = 0,[FromQuery] int take = 10)
        {
            return Ok(_nRepo.GetUsersReadNotifications(userId, skip, take));
        }

        /// <summary>
        ///     Method for getting information about paging on Read records
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>PagingViewModel</returns>
        [HttpGet("~/api/Notification/Read/Page/{userId}")]
        public ActionResult ReadPage(string userId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 1)
        {
            return Ok(_nRepo.GetReadPageDetails(userId, pageSize, pageIndex));
        }

        [HttpGet("~/api/Notification/Unread/Count/{userId}")]
        public ActionResult UnreadCount(string userId)
        {
            return Ok(_nRepo.GetUnReadNotificationCount(userId));
        }

        [HttpGet("{id}")]
        public ActionResult Mark(int id)
        {
            return Ok(_nRepo.MarkAsRead(id));
        }
    }
}
