using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.Repos.Interfaces
{
    /// <summary>
    ///     Role Respository Database API
    /// </summary>
    public interface IRoleRepo
    {
        /// <summary>
        ///     Gets all the roles in the Db
        /// </summary>
        /// <returns>Collection of IdentityRole</returns>
        IEnumerable<IdentityRole> GetRoles();

        /// <summary>
        /// Gets all the Users with the Role of Admin
        /// </summary>
        /// <returns>Collection of Users</returns>
        Task<IEnumerable<User>> GetAdmins();

        /// <summary>
        ///     Get All Users in the Role of Users
        /// </summary>
        /// <returns>Collection of users</returns>
        Task<IEnumerable<User>> GetUsers();

        /// <summary>
        ///     Adds a user to a specific role
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="roleId">Id of the role</param>
        /// <returns>true if succesful</returns>
        Task<bool> AddToRoleById(string userId, string roleId);

        /// <summary>
        ///     Adds a user to the user role
        /// </summary>
        /// <param name="userId">Id of ther user</param>
        /// <returns>true if succesful</returns>
        Task<bool> AddToUsers(string userId);

        /// <summary>
        ///     Adds a user to the admin role
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>true if succesful</returns>
        Task<bool> AddToAdmin(string userId);
    }
}
