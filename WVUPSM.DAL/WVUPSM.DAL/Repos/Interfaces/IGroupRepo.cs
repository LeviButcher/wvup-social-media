using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos.Interfaces
{
    /// <summary>
    ///     Group Respository Database API
    /// </summary>
    public interface IGroupRepo
    {
        /// <summary>
        ///     Gets a Single GroupRecord from the Database
        /// </summary>
        /// <param name="groupId">Id of the group to return</param>
        /// <returns>A group object</returns>
        Group GetBaseGroup(int groupId);

        /// <summary>
        ///     Gets a collection of UserProfiles of User's within a group
        /// </summary>
        /// <param name="groupId">Id of the group</param>
        /// <param name="skip">Amount of records to skip</param>
        /// <param name="take">Amount of records to take</param>
        /// <returns>A collection of user profiles within a group</returns>
        IEnumerable<UserProfile> GetGroupMembers(int groupId, int skip = 0, int take = 10);

        /// <summary>
        ///     Gets the number of how many members are in a group
        /// </summary>
        /// <param name="groupId">Id of the group</param>
        /// <returns>number of members in group</returns>
        int GetMemberCount(int groupId);

        /// <summary>
        ///     Gets a collection of GroupViewModels of the groups the user is in
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="skip">Amount of records to skip</param>
        /// <param name="take">Amount of records to take</param>
        /// <returns>Returns a collection of groupviewmodels</returns>
        IEnumerable<GroupViewModel> GetUsersGroups(string userId, int skip = 0, int take = 10);

        /// <summary>
        ///     Creates a new Group record
        /// </summary>
        /// <param name="group">Group to add to DB</param>
        /// <returns>1 if succesful, 0 otherwise</returns>
        int CreateGroup(Group group);

        /// <summary>
        ///     Deletes the Group from the DB
        /// </summary>
        /// <param name="group">Group to delete from DB</param>
        /// <returns>1 if succesful, 0 otherwise</returns>
        int DeleteGroup(Group group);

        /// <summary>
        ///     Updates a Group record in the DB
        /// </summary>
        /// <param name="group">Group record to update</param>
        /// <returns>1 if succesful, 0 otherwise</returns>
        int UpdateGroup(Group group);

        /// <summary>
        ///     Finds the groups whose name contains the term provided
        /// </summary>
        /// <param name="term">search term</param>
        /// <returns>collection of groupviewmodels</returns>
        IEnumerable<GroupViewModel> FindGroups(string term);

        /// <summary>
        ///     Gets the group whose id matches the id provided
        /// </summary>
        /// <param name="id">id of group</param>
        /// <returns>groupviewmdoel of group</returns>
        GroupViewModel GetGroup(int id);

        /// <summary>
        ///     Gets all the groups from the DB
        /// </summary>
        /// <returns>collection of groupviewmodels</returns>
        IEnumerable<GroupViewModel> GetAllGroups();

        /// <summary>
        ///     Checks if the user is the owner of this group
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <param name="groupId">id of the group</param>
        /// <returns>true if the user is the owner, false otherwise</returns>
        Task<bool> IsOwner(string userId, int groupId);

        /// <summary>
        ///     Gets the owner of the group
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <returns>UserProfile of the owner</returns>
        UserProfile GetOwner(int id);

        /// <summary>
        ///     Checks if the user is a member of the group
        /// </summary>
        /// <param name="userId">id of ther user</param>
        /// <param name="groupId">id of the group</param>
        /// <returns>true if the user is a member, false otherwise</returns>
        Task<bool> IsMember(string userId, int groupId);

        /// <summary>
        ///     Makes the user provided a member of this group
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <param name="groupId">id of the group</param>
        /// <returns>1 if succesful, false otherwise</returns>
        Task<int> JoinGroup(string userId, int groupId);

        /// <summary>
        ///     Makes the user provided leave the group
        /// </summary>
        /// <param name="userId">id of ther user</param>
        /// <param name="groupId">id of the group</param>
        /// <returns>1 if succesful, false otherwise</returns>
        Task<int> LeaveGroup(string userId, int groupId);

    }
}
