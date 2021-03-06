﻿using System;
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
    /// <summary>
    ///     Controller for Users
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private IUserRepo _uRepo;
        private UserManager<User> _uManager;
        private SignInManager<User> _signInManager;

        private ITagRepo _tagrepo { get; }

        public UserController(IUserRepo uRepo, UserManager<User> userManager, SignInManager<User> signInManager, ITagRepo tagrepo)
        {
            _uRepo = uRepo;
            _uManager = userManager;
            _signInManager = signInManager;
            _tagrepo = tagrepo;
        }

        /// <summary>
        ///     Allows a user to sign in
        /// </summary>
        /// <returns>That the user signed in or failed to do so</returns>
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

        /// <summary>
        ///     Allows a user to sign out
        /// </summary>
        /// <returns>Thhat the user signed out of the system</returns>
        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        ///     Deletes a user
        /// </summary>
        /// <returns>That the user was deleted or nothing</returns>
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

        /// <summary>
        ///     Gets a user
        /// </summary>
        /// <returns>The user</returns>
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

        /// <summary>
        ///     Gets a list of users
        /// </summary>
        /// <returns>The list of users</returns>
        [HttpGet]
        public IActionResult Get([FromQuery] int skip = 0, [FromQuery]  int take = 10)
        {
            var response = _uRepo.GetUsers(skip, take);
            return Ok(response);
        }

        /// <summary>
        ///     Creates a new user
        /// </summary>
        /// <returns>The new user</returns>
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

        /// <summary>
        ///     Updates a user
        /// </summary>
        /// <returns>The updated user or nothing</returns>
        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(string userId, [FromBody] UserProfile user)
        {

            if (user == null && userId != user.UserId) return NotFound();
            User userBase = await _uRepo.GetBase(user.UserId);

            //Update User's Properties here
            userBase.UserName = user.UserName;
            userBase.Email = user.Email;
            userBase.Bio = user.Bio;

            if(user.FileId > 0) userBase.FileId = user.FileId;
            if(user.HeaderPicId > 0) userBase.HeaderPicId = user.HeaderPicId;

            userBase.Major = user.Major;
            userBase.Occupation = user.Occupation;

            //Drop all tags assocatiated with user if tags provided, then call repo tag add methods
            if(user.Interests != null)
            {
                _tagrepo.DropAllUserTags(user.UserId);
                _tagrepo.CreateTags(user.Interests, user.UserId);
            }

            var result = await _uRepo.UpdateUserAsync(userBase);

            if (result == 1)
            {
                return Accepted(user);
            }
            return NoContent();
        }

        /// <summary>
        ///     Lets a user change their password
        /// </summary>
        /// <returns>The changed password or nothing</returns>
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

        /// <summary>
        ///     Finds a user
        /// </summary>
        /// <returns>The user</returns>
        [HttpGet("{term}")]
        public IActionResult Find(string term)
        {
            var users = _uRepo.FindUsers(term);
            return Ok(users);
        }
    }
}
