using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos.Interfaces
{
    /// <summary>
    ///     Post Respository Database API
    /// </summary>
    public interface IPostRepo
    {
        /// <summary>
        ///     Gets a UserPost matching the id
        /// </summary>
        /// <param name="id">post's id</param>
        /// <returns>Returns UserPost viewmodel matching that id</returns>
        UserPost GetPost(int id);

        /// <summary>
        ///     Gets a base post matching the id
        /// </summary>
        /// <param name="id">post's id</param>
        /// <returns>Returns post matching that id</returns>
        Post GetBasePost(int id);

        /// <summary>
        ///     Gets post's from the people that this user is following
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <param name="skip">Amount of records to skip</param>
        /// <param name="take">Amount of record to take</param>
        /// <returns>amount of UserPost less then or equal to take</returns>
        IEnumerable<UserPost> GetFollowingPosts(string userId, int skip = 0, int take = 10);

        /// <summary>
        ///     Deletes the post provided from the Database
        /// </summary>
        /// <param name="post">Post that must have a id and timestamp</param>
        /// <returns>number of records effected</returns>
        int DeletePost(Post post);

        /// <summary>
        ///     Creates a new post in the database
        /// </summary>
        /// <param name="post">A new post without a id</param>
        /// <returns>number of records created</returns>
        int CreatePost(Post post);

        /// <summary>
        ///     Gets the user's post matching the userId provided
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <param name="skip">Records to skip</param>
        /// <param name="take">Recrods to take</param>
        /// <returns>Amount of UserPost less then or equal to take</returns>
        IEnumerable<UserPost> GetUsersPost(string userId, int skip = 0, int take = 10);
    }
}
