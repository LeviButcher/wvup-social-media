using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos.Interfaces
{
    /// <summary>
    ///     Follow Repository Database API
    /// </summary>
    public interface IFollowRepo
    {
        /// <summary>
        ///     Gets the user's followers which are people following that user.
        /// </summary>
        /// <param name="userId">The id of this user</param>
        /// <param name="skip">Amount of records to skip</param>
        /// <param name="take">Amount of records to take</param>
        /// <returns>UserProfiles less then or equal to take</returns>
        IEnumerable<UserProfile> GetFollowers(string userId, int skip = 0, int take = 10);

        /// <summary>
        ///     Gets the people that the user is following.
        /// </summary>
        /// <param name="userId">The id of this user</param>
        /// <param name="skip">Amount of records to skip</param>
        /// <param name="take">Amount of records to take</param>
        /// <returns>UserProfiles less then or equal to take</returns>
        IEnumerable<UserProfile> GetFollowing(string userId, int skip = 0, int take = 10);

        /// <summary>
        ///     Creates a new Follow record
        /// </summary>
        /// <param name="follow">Follow object that MUST contain a userId and followId</param>
        /// <returns>integer value of number of records affected</returns>
        int CreateFollower(Follow follow);

        /// <summary>
        ///     Deletes a follow record
        /// </summary>
        /// <param name="follow">Follow object that MUST contain a userId and followId</param>
        /// <returns>value of number of records affected</returns>
        int DeleteFollower(Follow follow);

        /// <summary>
        ///     Gets a User's following count which is the number of people this user is following
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <returns>number of people user is following</returns>
        int GetFollowingCount(string userId);

        /// <summary>
        ///     Gets a User's follower count which is the number of people following this user
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <returns>number of people following this user</returns>
        int GetFollowerCount(string userId);

        /// <summary>
        ///     Determines if a user is following someone or not
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <param name="followId">Follow Id</param>
        /// <returns>True if the user is following the other person, false otherwise</returns>
        Task<bool> IsFollowingAsync(string userId, string followId);
    }
}
