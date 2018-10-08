using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVUPSM.MVC.Controllers
{
    [Route("[controller]/[action]")]
    public class MessageController : Controller
    {
        public IActionResult Inbox()
        {
            return View();
        }

        [Route("~/[controller]/{userId}")]
        public IActionResult Message(string userId)
        {
            //Created a base MessageViewModel here for Form
            //Get the current user
            //built out message view model senderId to current user
            //build out messsageviewmodel recieverId to userId
            return View();
        }
    }
}
