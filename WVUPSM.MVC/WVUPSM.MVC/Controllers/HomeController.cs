using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.Models;

namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<UserPost> posts = new List<UserPost>();
            UserPost post = new UserPost()
            {
                DateCreated = new DateTime(),
                Email = "lbutche3@wvup.edu",
                Text = "I saw a cat the other day",
                UserName = "LeviButcher",
            };
            posts.Add(post);


            return View(posts);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Search(string term)
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }
    }
}
