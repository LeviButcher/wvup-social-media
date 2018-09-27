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
    public class GroupRepo : IGroupRepo
    {
        private readonly SMContext Db;
        public DbSet<Group> Table;
        public DbSet<UserGroup> UserGroupTable;
        public SMContext Context => Db;
        UserRepo userRepo;

        public GroupRepo()
        {
            Db = new SMContext();
            Table = Db.Set<Group>();
            userRepo = new UserRepo();
            UserGroupTable = Db.Set<UserGroup>();
        }

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

        public Group GetBaseGroup(int groupId) 
            => Table.First(x => x.Id == groupId);


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

        public static GroupViewModel GetGroupRecord(Group group)
         => new GroupViewModel()
         {
             OwnerId = group.OwnerId,
             Bio = group.Bio,
             GroupName = group.Name,
             DateCreated = group.DateCreated,
             GroupId = group.Id,
             MemberCount = group.Members.Count
         };

        public GroupViewModel GetGroup(int id)
        {
            var group = Table.Include(e => e.Members)
                .First(x => x.Id == id);
            return group == null ? null : GetGroupRecord(group);
        }


        public IEnumerable<UserProfile> GetGroupMembers(int groupId, int skip = 0, int take = 10)
        {
            return Table.Include(x => x.Members)
                .Where(x => x.Members.All(z => z.GroupId == groupId))
                .Select(item => userRepo.GetRecord(item.User, item.User.Following, item.User.Followers));
        }

        public int GetMemberCount(int groupId)
        {
            var group = GetGroup(groupId);
            return group.MemberCount;
        }

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

        public int DeleteGroup(Group group)
        {
            Table.Remove(group);
            return this.SaveChanges();
        }

        public int UpdateGroup(Group group)
        {
            Table.Update(group);
            return this.SaveChanges();
        }
        
        public IEnumerable<GroupViewModel> GetAllGroups()
        {
            return Table.Include(x => x.Members)
                 .Select(item => GetGroupRecord(item));
        }

        public async Task<bool> IsOwner(string userId, int groupId)
        {
            return await Table.AnyAsync(x => x.Id == groupId && x.OwnerId == userId);
        }

        public UserProfile GetOwner(int id)
        {
            return Table.Include(x => x.OwnerId)
                .Where(x => x.Id == id)
                .Select(item => userRepo.GetUser(item.OwnerId))
                .FirstOrDefault();               
        }

        public async Task<bool> IsMember(string userId, int groupId)
        {
            return await UserGroupTable.AnyAsync(x => x.UserId == userId && x.GroupId == groupId);
        }

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
