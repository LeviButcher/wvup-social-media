using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.Service.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserRepo _uRepo;

        public UserController(IUserRepo uRepo)
        {
            _uRepo = uRepo;
        }

        public IActionResult SignIn(LoginViewModel model)
        {
            return null;
        }

        public IActionResult SignOut()
        {
            return null;
        }

        [HttpDelete]
        public IActionResult Delete(string userId, UserProfile user)
        {
            return null;
        }

        [HttpGet]
        public IActionResult Get(string userId)
        {
            var item = _uRepo.GetUser(userId);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }

        [HttpGet]
        public IActionResult Get(int skip = 0, int take = 10)
        {
            return Ok(_uRepo.GetUsers(skip, take));
        }

        [HttpPost]
        public IActionResult Create(User user, string password)
        {
            return null;
        }

        [HttpPut]
        public IActionResult Update(string userId, UserProfile user)
        {
            return null;
        }

        [HttpPut]
        public IActionResult ChangePassword(string userId, UserProfile user, string currPassword, string newPassword)
        {
            return null;
        }

        [HttpGet]
        public IActionResult Find(string term)
        {
            return Ok(_uRepo.FindUsers(term));
        }
    }
}
