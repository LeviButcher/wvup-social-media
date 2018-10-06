using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.Service.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private IUserRepo _uRepo;
        private UserManager<User> _uManager;
        private SignInManager<User> _signInManager;

        public UserController(IUserRepo uRepo, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _uRepo = uRepo;
            _uManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] LoginViewModel model)
        {
            var user = await _uManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound();

            var result = await _signInManager.PasswordSignInAsync(user, model.password, false, false);
            if (result.Succeeded)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            User userBase = await _uRepo.GetBase(userId);

            var result = await _uManager.DeleteAsync(userBase);
            if (result.Succeeded)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("{userId}")]
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
        public IActionResult Get([FromQuery] int skip = 0, [FromQuery]  int take = 10)
        {
            var response = _uRepo.GetUsers(skip, take);
            return Ok(response);
        }

        [HttpPost("{password}")]
        public async Task<IActionResult> Create(string password, [FromBody] User user)
        {
            var result = await _uManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return Created($"api/User/Get/{user.Id}", user);
            }

            return NotFound();
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(string userId, [FromBody] UserProfile user)
        {
            
            if (user == null && userId != user.UserId) return NotFound();
            User userBase = await _uRepo.GetBase(user.UserId);

            //Update User's Properties here
            userBase.UserName = user.UserName;
            userBase.Email = user.Email;
            userBase.Bio = user.Bio;
            var result = await _uRepo.UpdateUserAsync(userBase);

            if (result == 1)
            {
                return Accepted(user);
            }
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangePassword(string userId, UserProfile user, string currPassword, string newPassword)
        {
            if (user == null && userId != user.UserId) return NotFound();
            User userBase = await _uRepo.GetBase(user.UserId);
           
            var result = await _uManager.ChangePasswordAsync(userBase, currPassword, newPassword);
            if (result.Succeeded)
            {
                return Accepted();
            }
            return NoContent();
        }

        [HttpGet("{term}")]
        public IActionResult Find(string term)
        {
            var users = _uRepo.FindUsers(term);
            return Ok(users);
        }
    }
}
