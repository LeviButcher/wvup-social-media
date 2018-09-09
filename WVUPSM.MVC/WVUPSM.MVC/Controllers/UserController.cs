using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.Models;

namespace WVUPSM.MVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(string userId)
        {
            return View();
        }

        public IActionResult Followers(string userId)
        {
            return View();
        }

        public IActionResult Following(string userId)
        {
            return View();
        }

        public IActionResult ToggleFollow(string userId, string followId, Follow follow)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Delete(string userId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string userId, UserProfile user, bool confirmed)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string userId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(string userId, User user)
        {
            return View();
        }
    }
}