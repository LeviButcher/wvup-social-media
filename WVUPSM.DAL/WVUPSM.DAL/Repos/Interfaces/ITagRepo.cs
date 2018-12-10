using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.Repos.Interfaces
{
    /// <summary>
    ///     Tag Respository Database API
    /// </summary>
    public interface ITagRepo
    {

        /// <summary>
        ///     Creates a new Tag record
        /// </summary>
        /// <param name="tag">tag to add to DB</param>
        /// <param name="userId"></param>
        /// <returns>1 if succesful, 0 otherwise</returns>
        int CreateTag(string name, string userId);

        /// <summary>
        ///     Creates multiple tags and assocates them to a user
        /// </summary>
        /// <param name="spaceDelimitedTags"></param>
        /// <param name="userId"></param>
        /// <returns>1 if successful, 0 otherwise</returns>
        int CreateTags(string spaceDelimitedTags, string userId);

        /// <summary>
        ///     Removes all tags assocatiated with this user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>1 if successful, 0 otherwise</returns>
        int DropAllUserTags(string userId);

        /// <summary>
        ///     Deletes the Tag from the DB
        /// </summary>
        /// <param name="tag">tag to delete from DB</param>
        /// <returns>1 if succesful, 0 otherwise</returns>
        int DeleteTag(Tag tag);

        /// <summary>
        ///    Returns UserTags for passed in User
        /// </summary>
        /// <param name="user">User to find in DB</param>
        /// <returns>List of UserTags for User</returns>
        IEnumerable<UserTag> GetUserTagsByUser(string userId);

        /// <summary>
        ///     Checks if Tag exists in DB
        /// </summary>
        /// <param name="word">word to search in DB</param>
        /// <returns>true if tag exists, else false</returns>
        bool IsTag(string word);

        /// <summary>
        ///    Returns UserTags for passed in Tag
        /// </summary>
        /// <param name="tag">User to find in DB</param>
        /// <returns>List of UserTags for Tag</returns>
        IEnumerable<UserTag> GetUserTagsByTag(int tagId);

        /// <summary>
        ///  Removes tag when user deletes it from their interests
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int RemoveUserTag(int tagId, string userId);

        /// <summary>
        ///   Determines if UserTag object exists
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsUserTag(int tagId, string userId);

        /// <summary>
        ///   Returns a list of all tags in Db matching search term
        /// </summary>
        /// <param name="term">term to be searched</param>
        /// <returns>A list of all Tags</returns>
        IEnumerable<Tag> FindTags(string term);

    }
}
