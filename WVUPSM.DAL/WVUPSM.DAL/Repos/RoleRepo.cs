using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.DAL.EF;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.Repos
{
    /// <summary>
    ///     Role Repository for SQL Server
    /// </summary>
    public class RoleRepo : IRoleRepo
    {
        protected readonly SMContext Db;
        protected DbSet<IdentityRole> Table;
        public SMContext Context => Db;

        public UserManager<User> _UserManager { get; }
        public RoleManager<IdentityRole> _RoleManager { get; }

        public RoleRepo(UserManager<User> _userManager, RoleManager<IdentityRole> roleManager)
        {
            Db = new SMContext();
            Table = Db.Roles;
            _UserManager = _userManager;
            _RoleManager = roleManager;
        }

        public RoleRepo(DbContextOptions<SMContext> options, UserManager<User> _userManager, RoleManager<IdentityRole> roleManager)
        {
            Db = new SMContext(options);
            Table = Db.Roles;
            _UserManager = _userManager;
            _RoleManager = roleManager;
        }

        /// <summary>
        ///     Adds a user to the admin role
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>true if succesful</returns>
        public async Task<bool> AddToAdmin(string userId)
        {
            return await AddToRole(userId, "Admin");
        }

        /// <summary>
        ///     Adds a user to a specific role
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="roleId">Id of the role</param>
        /// <returns>true if succesful</returns>
        public async Task<bool> AddToRoleById(string userId, string roleId)
        {
            var role = await _RoleManager.FindByIdAsync(roleId);
            return await AddToRole(userId, role.Id);
        }

        /// <summary>
        ///     Adds a user to the user role
        /// </summary>
        /// <param name="userId">Id of ther user</param>
        /// <returns>true if succesful</returns>
        public async Task<bool> AddToUsers(string userId)
        {
            return await AddToRole(userId, "User");
        }

        /// <summary>
        /// Gets all the Users with the Role of Admin
        /// </summary>
        /// <returns>Collection of Users</returns>
        public async Task<IEnumerable<User>> GetAdmins()
        {
            return await _UserManager.GetUsersInRoleAsync("Admin");
        }

        /// <summary>
        ///     Gets all the roles in the Db
        /// </summary>
        /// <returns>Collection of IdentityRole</returns>
        public IEnumerable<IdentityRole> GetRoles() => Table;

        /// <summary>
        ///     Get All Users in the Role of Users
        /// </summary>
        /// <returns>Collection of users</returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _UserManager.GetUsersInRoleAsync("User");
        }

        /// <summary>
        ///     Adds a user to a specific role
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="roleId">Id of the role</param>
        /// <returns>true if succesful</returns>
        internal async Task<bool> AddToRole(string Id, string roleName)
        {
            var user = await _UserManager.FindByIdAsync(Id);
            var allRoles = await _UserManager.GetRolesAsync(user);
            await _UserManager.RemoveFromRolesAsync(user, allRoles);

            var result = await _UserManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }
    }
}
