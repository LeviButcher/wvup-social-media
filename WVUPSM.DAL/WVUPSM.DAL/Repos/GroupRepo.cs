using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos
{

    /// <summary>
    ///     Group Respository for SQL Server implemenation
    /// </summary>
    public class GroupRepo : IGroupRepo
    {
        private readonly SMContext Db;
        public DbSet<Group> Table;
        public DbSet<UserGroup> UserGroupTable;
        public SMContext Context => Db;
        UserRepo userRepo;

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public GroupRepo()
        {
            Db = new SMContext();
            Table = Db.Set<Group>();
            userRepo = new UserRepo();
            UserGroupTable = Db.Set<UserGroup>();
        }

        /// <summary>
        ///     Overloaded Constructor, used by dependcy injection when a connection string is provided
        /// </summary>
        /// <param name="options"></param>
        public GroupRepo(DbContextOptions<SMContext> options)
        {
            Db = new SMContext(options);
            Table = Db.Set<Group>();
            userRepo = new UserRepo();
            UserGroupTable = Db.Set<UserGroup>();
        }

        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                //Free any other managed objects here
            }
            Db.Dispose();
            _disposed = true;
        }

        /// <summary>
        ///     Saves changes to DB
        /// </summary>
        /// <returns>1 if successful, 0 if not</returns>
        public int SaveChanges()
        {
            try
            {
                return Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A concurrency error occurred
                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                //DbResiliency retry limit exceeded
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        ///     Gets a Single GroupRecord from the Database
        /// </summary>
        /// <param name="groupId">Id of the group to return</param>
        /// <returns>A group object</returns>
        public Group GetBaseGroup(int groupId) 
            => Table.First(x => x.Id == groupId);


        /// <summary>
        ///     Finds the groups whose name contains the term provided
        /// </summary>
        /// <param name="term">search term</param>
        /// <returns>collection of groupviewmodels</returns>
        public IEnumerable<GroupViewModel> FindGroups(string term)
        {
            var results = Table
               .Where(e => e.Name.ToUpper().Contains(term.ToUpper()));
            List<GroupViewModel> foundGroups = new List<GroupViewModel>();

            foreach (Group group in results)
            {
                foundGroups.Add(GetGroupRecord(group));
            }
            return foundGroups;
        }

        /// <summary>
        ///     Return a GroupViewModel of the group
        /// </summary>
        /// <param name="group">Group to convert to viewmodel</param>
        /// <returns>Return GroupViewModel</returns>
        public static GroupViewModel GetGroupRecord(Group group)
         => new GroupViewModel()
         {
             OwnerId = group.OwnerId,
             Bio = group.Bio,
             GroupName = group.Name,
             DateCreated = group.DateCreated,
             GroupId = group.Id,
             MemberCount = group.Members.Count,
         };

        /// <summary>
        ///     Gets the group whose id matches the id provided
        /// </summary>
        /// <param name="id">id of group</param>
        /// <returns>groupviewmdoel of group</returns>
        public GroupViewModel GetGroup(int id)
        {
            var group = Table.Include(e => e.Members)
                .First(x => x.Id == id);
            return group == null ? null : GetGroupRecord(group);
        }

        /// <summary>
        ///     Gets a collection of UserProfiles of User's within a group
        /// </summary>
        /// <param name="groupId">Id of the group</param>
        /// <param name="skip">Amount of records to skip</param>
        /// <param name="take">Amount of records to take</param>
        /// <returns>A collection of user profiles within a group</returns>
        public IEnumerable<UserProfile> GetGroupMembers(int groupId, int skip = 0, int take = 10)
        {
           return UserGroupTable.Include(x => x.User).ThenInclude(x => x.Followers).Include(x => x.User).ThenInclude(x => x.Following)
                .Where(x => x.GroupId == groupId)
                .Select(item => userRepo.GetRecord(item.User, item.User.Following, item.User.Followers));
            //return Table.Include(x => x.Members).ThenInclude(x => x.UserId)
            //    .Where(x => x.Members.Any(z => z.GroupId == groupId))
            //    .Select(item => userRepo.GetRecord(item.User, item.User.Following, item.User.Followers));
        }

        /// <summary>
        ///     Gets the number of how many members are in a group
        /// </summary>
        /// <param name="groupId">Id of the group</param>
        /// <returns>number of members in group</returns>
        public int GetMemberCount(int groupId)
        {
            var group = GetGroup(groupId);
            return group.MemberCount;
        }

        /// <summary>
        ///     Gets a collection of GroupViewModels of the groups the user is in
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="skip">Amount of records to skip</param>
        /// <param name="take">Amount of records to take</param>
        /// <returns>Returns a collection of groupviewmodels</returns>
        public IEnumerable<GroupViewModel> GetUsersGroups(string userId, int skip = 0, int take = 10)
        {
            return Table.Include(x => x.Members)
                .Where(x => x.Members.Any(z => z.UserId == userId))
                .Select(item => GetGroupRecord(item));

            //var user = await userRepo.GetBase(userId);
            //var userGroup = user.Groups;
            //List<GroupViewModel> groupList = new List<GroupViewModel>();
            //foreach(UserGroup userGroups in userGroup)
            //{
            //    groupList.Add(GetGroup(userGroups.Group.Id));
            //}
            //return groupList;
            
        }

        /// <summary>
        ///     Creates a new Group record
        /// </summary>
        /// <param name="group">Group to add to DB</param>
        /// <returns>1 if succesful, 0 otherwise</returns>
        public int CreateGroup(Group group)
        {
            Table.Add(group);
            UserGroupTable.Add(new UserGroup
            {
                UserId = group.OwnerId,
                GroupId = group.Id
            }); 
            
            return this.SaveChanges();
        }

        /// <summary>
        ///     Deletes the Group from the DB
        /// </summary>
        /// <param name="group">Group to delete from DB</param>
        /// <returns>1 if succesful, 0 otherwise</returns>
        public int DeleteGroup(Group group)
        {
            Table.Remove(group);
            return this.SaveChanges();
        }

        /// <summary>
        ///     Updates a Group record in the DB
        /// </summary>
        /// <param name="group">Group record to update</param>
        /// <returns>1 if succesful, 0 otherwise</returns>
        public int UpdateGroup(Group group)
        {
            Table.Update(group);
            return this.SaveChanges();
        }

        /// <summary>
        ///     Gets all the groups from the DB
        /// </summary>
        /// <returns>collection of groupviewmodels</returns>
        public IEnumerable<GroupViewModel> GetAllGroups()
        {
            return Table.Include(x => x.Members)
                 .Select(item => GetGroupRecord(item));
        }


        /// <summary>
        ///     Checks if the user is the owner of this group
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <param name="groupId">id of the group</param>
        /// <returns>true if the user is the owner, false otherwise</returns>
        public async Task<bool> IsOwner(string userId, int groupId)
        {
            return await Table.AnyAsync(x => x.Id == groupId && x.OwnerId == userId);
        }

        /// <summary>
        ///     Gets the owner of the group
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <returns>UserProfile of the owner</returns>
        public UserProfile GetOwner(int id)
        {
            return Table.Include(x => x.OwnerId)
                .Where(x => x.Id == id)
                .Select(item => userRepo.GetUser(item.OwnerId))
                .FirstOrDefault();               
        }

        /// <summary>
        ///     Checks if the user is a member of the group
        /// </summary>
        /// <param name="userId">id of ther user</param>
        /// <param name="groupId">id of the group</param>
        /// <returns>true if the user is a member, false otherwise</returns>
        public async Task<bool> IsMember(string userId, int groupId)
        {
            return await UserGroupTable.AnyAsync(x => x.UserId == userId && x.GroupId == groupId);
        }

        /// <summary>
        ///     Makes the user provided a member of this group
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <param name="groupId">id of the group</param>
        /// <returns>1 if succesful, false otherwise</returns>
        public async Task<int> JoinGroup(string userId, int groupId)
        {
            var isMemberCheck = await IsMember(userId, groupId);
            if(!isMemberCheck)
            {
                var join = new UserGroup
                {
                    GroupId = groupId,
                    UserId = userId
                };
                
                Db.UserGroups.Add(join);
                return Db.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        ///     Makes the user provided leave the group
        /// </summary>
        /// <param name="userId">id of ther user</param>
        /// <param name="groupId">id of the group</param>
        /// <returns>1 if succesful, false otherwise</returns>
        public async Task<int> LeaveGroup(string userId, int groupId)
        {
            var isMemberCheck = await IsMember(userId, groupId);
            if (isMemberCheck)
            {
                var join = new UserGroup
                {
                    GroupId = groupId,
                    UserId = userId
                };

                Db.Remove(join);
                return Db.SaveChanges();
            }
            return 0;
        }
    }
}
