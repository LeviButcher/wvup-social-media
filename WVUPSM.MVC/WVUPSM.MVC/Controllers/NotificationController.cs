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
        public async Task<ActionResult> Index([FromQuery] string tab, [FromQuery] int? page)
        {
            ViewData["tab"] = tab ?? "";
            PagingViewModel model;
            if (tab == "Read")
            {
                 model = await _api.GetReadPageDetails(_userManager.GetUserId(User), 10, page ?? 1);
            }
            else
            {
                model = await _api.GetUnreadPageDetails(_userManager.GetUserId(User), 10, page ?? 1);
            }

            return View(model);
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
