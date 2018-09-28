using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace WVUPSM.DAL.Repos
{
    public class UserRepo : IUserRepo
    {
        public readonly SMContext Db;
        public DbSet<User> Table { get; }

        public UserRepo()
        {
            Db = new SMContext();
            Table = Db.Set<User>();
        }

        protected UserRepo(DbContextOptions<SMContext> options)
        {
            Db = new SMContext(options);
            Table = Db.Set<User>();
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

        public async Task<User> GetBase(string id)
        {
            return await Table.FindAsync(id);
        }

        public IEnumerable<UserProfile> FindUsers(string term)
        {
            var results = Table
                .Where(e => e.UserName.ToUpper().Contains(term.ToUpper()) || e.Email.ToUpper().Contains(term.ToUpper()));

            List<UserProfile> returnProfiles = new List<UserProfile>();
            foreach (User user in results)
            {
                returnProfiles.Add(GetUser(user.Id));
            }
            return returnProfiles;
        }

        public UserProfile GetRecord(User user, IEnumerable<Follow> following, IEnumerable<Follow> followers)
            => new UserProfile()
            {
                Email = user.Email,
                UserId = user.Id,
                UserName = user.UserName,
                FollowerCount = followers != null ? followers.Count() : 0,
                FollowingCount = following != null ? following.Count() : 0,
                Bio = user.Bio
            };

        public IEnumerable<UserProfile> GetAllUsers()
        {
            return Table.Include(x => x.Following).Include(x => x.Followers)
                .OrderBy(x => x.UserName)
                .Select(item => GetRecord(item, item.Following, item.Followers));
        }

        public UserProfile GetUser(string id)
        {
            var user = Table.Include(e => e.Following).Include(e => e.Followers)
                .First(x => x.Id == id);

            return user == null ? null : GetRecord(user, user.Following, user.Followers);
        }

        public IEnumerable<UserProfile> GetUsers(int skip = 0, int take = 10)
        {
            return Table.Include(e => e.Following).Include(e => e.Followers)
                        .Skip(skip).Take(take)
                        .OrderBy(x => x.UserName)
                        .Select(item => GetRecord(item, item.Following, item.Followers));
        }


        public async Task<int> UpdateUserAsync(User user)
        {
            Table.Update(user);

            return SaveChanges();
        }

        public IEnumerable<Group> GetGroups(User user)
        {
            IEnumerable<UserGroup> userGroups = user.Groups;
            List<Group> groupList = null;
            foreach(UserGroup userGroup in userGroups)
            {
                groupList.Add(userGroup.Group);
            }
            return groupList;
        }

    }
}