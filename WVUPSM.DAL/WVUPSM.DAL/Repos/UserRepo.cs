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
    /// <summary>
    ///     User Repository implementing IUserRepo
    /// </summary>
    public class UserRepo : IUserRepo
    {
        private readonly SMContext _db;

        /// <summary>
        ///     User Table in database
        /// </summary>
        public DbSet<User> Table { get; }

        /// <summary>
        ///     Repo Constructor
        /// </summary>
        public UserRepo()
        {
            _db = new SMContext();
            Table = _db.Set<User>();
        }

        /// <summary>
        ///     Overloaded Constructor
        /// </summary>
        /// <param name="options">DbContextOptions</param>
        public UserRepo(DbContextOptions<SMContext> options)
        {
            _db = new SMContext(options);
            Table = _db.Set<User>();
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
            _db.Dispose();
            _disposed = true;
        }

        public int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A concurrency error occurred
                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                //_dbResiliency retry limit exceeded
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
        ///     Returns a User record
        /// </summary>
        /// <param name="id">User's Id</param>
        /// <returns>User</returns>
        public async Task<User> GetBase(string id)
        {
            return await Table.FindAsync(id);
        }


        /// <summary>
        ///    Search for users by term
        /// </summary>
        /// <param name="term">Search Term</param>
        /// <returns>List of UserProfiles</returns>
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

        /// <summary>
        ///    Gets a UserProfile ViewModel based on passed in User
        /// </summary>
        /// <param name="user">The user who's UserProfile is needed</param>
        /// <param name="following">User's Following List</param>
        /// <param name="followers">User's Follower List</param>
        /// <returns>UserProfile</returns>
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


        /// <summary>
        ///    Gets all Users
        /// </summary>
        /// <returns>List of UserProfiles</returns>
        public IEnumerable<UserProfile> GetAllUsers()
        {
            return Table.Include(x => x.Following).Include(x => x.Followers)
                .OrderBy(x => x.UserName)
                .Select(item => GetRecord(item, item.Following, item.Followers));
        }

        /// <summary>
        ///    Gets one User's UserProfile
        /// </summary>
        /// <param name="id">The user who's UserProfile is needed</param>
        /// <returns>UserProfile</returns>
        public UserProfile GetUser(string id)
        {
            var user = Table.Include(e => e.Following).Include(e => e.Followers)
                .First(x => x.Id == id);

            return user == null ? null : GetRecord(user, user.Following, user.Followers);
        }


        /// <summary>
        ///    Get Users with Paging
        /// </summary>
        /// <param name="skip">Number of Users to skip each time, default is 0</param>
        /// <param name="take">Number of Users to take each time, default is 10</param>
        /// <returns>List of UserProfiles</returns>
        public IEnumerable<UserProfile> GetUsers(int skip = 0, int take = 10)
        {
            return Table.Include(e => e.Following).Include(e => e.Followers)
                        .Skip(skip).Take(take)
                        .OrderBy(x => x.UserName)
                        .Select(item => GetRecord(item, item.Following, item.Followers));
        }

        /// <summary>
        ///    Update User in Database
        /// </summary>
        /// <param name="user">User to be updated</param>
        /// <returns>Number of affected records</returns>
        public async Task<int> UpdateUserAsync(User user)
        {
            Table.Update(user);

            return SaveChanges();
        }


        /// <summary>
        ///    Gets Groups a User is a member of
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>List of Groups</returns>
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
