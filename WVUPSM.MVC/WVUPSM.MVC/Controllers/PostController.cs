using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Me()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult Delete(int postId, UserPost post, bool confirmed)
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Edit(int postId, UserPost post)
        {
            return View();
        }
    }
}
