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
    ///     Follow Repository implementing IFollowRepo
    /// </summary>
    public class FollowRepo : IFollowRepo
    {
       
        private readonly SMContext _db;

        /// <summary>
        ///     Comment Table in database
        /// </summary>
        public DbSet<Follow> Table;

        /// <summary>
        ///     Database context
        /// </summary>
        public SMContext Context => _db;
        UserRepo userRepo;

        /// <summary>
        ///     Repo Constructor
        /// </summary>
        public FollowRepo()
        {
            _db = new SMContext();
            Table = _db.Set<Follow>();
            userRepo = new UserRepo();
        }

        /// <summary>
        ///     Overloaded Constructor
        /// </summary>
        protected FollowRepo(DbContextOptions<SMContext> options)
        {
            _db = new SMContext(options);
            Table = _db.Set<Follow>();
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

        /// <summary>
        ///     Saves Changes within Database
        /// </summary>
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
        ///     Gets all of a User's Followers based on userId
        /// </summary>
        ///  <param name="userId">userId</param>
        ///  <param name="skip">the number of comments to skip. default is 0</param>
        ///  <param name="take">the number of comments to take, default is 10</param>
        /// <returns>A list of UserProfiles</returns>
        public IEnumerable<UserProfile> GetFollowers(string userId, int skip = 0, int take = 10)
        {
            return Table.Include(e => e.User)
                .Where(x => x.FollowId == userId)
                .Skip(skip).Take(take)
                .OrderBy(x => x.User.UserName)
                .Select(item => userRepo.GetUser(item.UserId));
        }

        /// <summary>
        ///     Gets all who are Following a User based on userId
        /// </summary>
        ///  <param name="userId">userId</param>
        ///  <param name="skip">the number of comments to skip. default is 0</param>
        ///  <param name="take">the number of comments to take, default is 10</param>
        /// <returns>A list of UserProfiles</returns>
        public IEnumerable<UserProfile> GetFollowing(string userId, int skip = 0, int take = 10)
        {
            return Table.Include(e => e.User)
                .Where(x => x.UserId == userId)
                .Skip(skip).Take(take)
                .OrderBy(x => x.User.UserName)
                .Select(item => userRepo.GetUser(item.FollowId));
        }

        /// <summary>
        ///     Creates a new Follow
        /// </summary>
        /// <param name="follow">Follow to be created</param>
        /// <returns>Number of affected records</returns>
        public int CreateFollower(Follow follow)
        {
            Table.Add(follow);
            return this.SaveChanges();
        }

        /// <summary>
        ///     Deletes Follow
        /// </summary>
        /// <param name="follow">Follow to be created</param>
        /// <returns>Number of affected records</returns>
        public int DeleteFollower(Follow follow)
        {
            Table.Remove(follow);
            return this.SaveChanges();
        }

        /// <summary>
        ///     Gets number of users following a User based on userId
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Number of other Users Following passed in User</returns>
        public int GetFollowingCount(string userId)
        {
            return Table.Count(x => x.UserId == userId);
        }

        /// <summary>
        ///     Gets number a Users followers suserId
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Number of other Users Following passed in User</returns>
        public int GetFollowerCount(string userId)
        {
            return Table.Count(x => x.FollowId == userId);
        }

        public async Task<bool> IsFollowingAsync(string userId, string followId)
        {
            return await Table.AnyAsync(x => x.UserId == userId && x.FollowId == followId);
        }
    }
}