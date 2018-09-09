using Microsoft.AspNetCore.Identity;
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
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private IUserRepo _uRepo;
        private UserManager<User> uManager;
        private SignInManager<User> _signInManager;

        public UserController(IUserRepo uRepo, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _uRepo = uRepo;
            uManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            var user = await uManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound();

            var result = await _signInManager.PasswordSignInAsync(user, model.password, false, false);
            if (result.Succeeded)
            {
                return Ok();
            }

            return NotFound();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId, [FromBody] UserProfile user)
        {
            if (user == null && userId != user.UserId) return NotFound();
            User userBase = await _uRepo.GetBase(user.UserId);

            var result = await uManager.DeleteAsync(userBase);
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
        public IActionResult Get(int skip = 0, int take = 10)
        {
            return Ok(_uRepo.GetUsers(skip, take));
        }

        [HttpPost("{password}")]
        public async Task<IActionResult> Create(string password, [FromBody] User user)
        {
            var result = await uManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return Created($"api/User/Get/{user.Id}", user);
            }

            return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Update(string userId, UserProfile user)
        {
            if (user == null && userId != user.UserId) return NotFound();
            User userBase = await _uRepo.GetBase(user.UserId);

            var result = await uManager.UpdateAsync(userBase);
            if (result.Succeeded)
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

            var result = await uManager.ChangePasswordAsync(userBase, currPassword, newPassword);
            if (result.Succeeded)
            {
                return Accepted();
            }
            return NoContent();
        }

        [HttpGet]
        public IActionResult Find(string term)
        {
            return Ok(_uRepo.FindUsers(term));
        }
    }
}
