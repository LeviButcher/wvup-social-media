using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.WebServiceAccess;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class NotificationController : Controller
    {

        public IWebApiCalls _api { get; }
        public UserManager<User> _userManager { get; }

        public NotificationController(IWebApiCalls api, UserManager<User> userManager)
        {
            _api = api;
            _userManager = userManager;
        }

        
        [HttpGet]
        public ActionResult Index([FromQuery] string tab)
        {
            ViewData["tab"] = tab ?? "";
            return View();
        }

        [HttpPost("{notificationId}")]
        public async Task<ActionResult> Mark(int notificationId)
        {
            return Ok(await _api.MarkAsRead(notificationId));
        }

        [HttpGet]
        public async Task<Int64> UnReadCount()
        {
            var userId = _userManager.GetUserId(User);
            return await _api.GetUnreadCount(userId);
        }
    }
}
