using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.ViewModels;
using WVUPSM.Models.Entities;
using System.Threading.Tasks;

namespace WVUPSM.DAL.Repos.Interfaces
{
    /// <summary>
    ///     User Respository Database API
    /// </summary>
    public interface IUserRepo
    {
        /// <summary>
        ///     Get all users 
        /// </summary>
        /// <returns>All users</returns>
        IEnumerable<UserProfile> GetAllUsers();

        /// <summary>
        ///     Get users
        /// </summary>
        /// <param name="skip">amount of records to skip</param>
        /// <param name="take">amount of records to take</param>
        /// <returns>UserProfiles less than or equal to take</returns>
        IEnumerable<UserProfile> GetUsers(int skip = 0, int take = 10);

        /// <summary>
        ///     Finds a user matching any word or subcharacter within email and userName
        /// </summary>
        /// <param name="term">letter[s] to search for</param>
        /// <returns>0 or more UserProfile matching the term</returns>
        IEnumerable<UserProfile> FindUsers(string term);

        /// <summary>
        ///     Gets a User with the id provided
        /// </summary>
        /// <param name="id">user's Id</param>
        /// <returns>User with matching id or null</returns>
        Task<User> GetBase(string id);

        /// <summary>
        ///     Gets a userProfile with the id provided
        /// </summary>
        /// <param name="id">user's id</param>
        /// <returns>UserProfile with matching id or null</returns>
        UserProfile GetUser(string id);
        //Task<bool> DeleteUserAsync(User user);
        /// <summary>
        ///     Updates a user if the user exists in the database
        /// </summary>
        /// <param name="user">User with a Id</param>
        /// <returns>Amount of records effected</returns>
        Task<int> UpdateUserAsync(User user);
        //Task<UserProfile> CreateUserAsync(User user, String password);
        //Task<bool> ChangePasswordAsync(User user, string currPass, string newPass);
    }
}
