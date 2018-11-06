using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WVUPSM.MVC.Models;
using WVUPSM.MVC.Service;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class RSSController : Controller
    {
        public RSSController()
        {
            
        }

        /// <summary>
        ///     News Index Page 
        ///     Displays RSS content
        /// </summary>
        /// <param name="rssUrl">URL of RSS post</param>
        [HttpGet]
        public IActionResult Index([FromQuery] string rssUrl, [FromQuery] string tab)
        {
            RSSFeedService service = new RSSFeedService();
            List<RssFeedItemViewModel> rssFeed = new List<RssFeedItemViewModel>();
            ViewData["tab"] = tab ?? "";

            rssUrl = "https://thewvupchronicle.com/feed/";
            if(tab == "WVUP")
            {
                rssUrl = "http://www.wvup.edu/feed/";
            }

            rssFeed = service.ParseRssDoc(rssUrl);
            return View(rssFeed);
        }
    }
}
