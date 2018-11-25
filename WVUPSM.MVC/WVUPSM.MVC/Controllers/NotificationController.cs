using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public NotificationController(IWebApiCalls api)
        {
            _api = api;
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
    }
}
