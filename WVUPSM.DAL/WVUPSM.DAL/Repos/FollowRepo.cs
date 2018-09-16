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
    public class FollowRepo : IFollowRepo
    {
        private readonly SMContext Db;
        public DbSet<Follow> Table;
        public SMContext Context => Db;
        UserRepo userRepo;

        public FollowRepo()
        {
            Db = new SMContext();
            Table = Db.Set<Follow>();
            userRepo = new UserRepo();
        }

        protected FollowRepo(DbContextOptions<SMContext> options)
        {
            Db = new SMContext(options);
            Table = Db.Set<Follow>();
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

          

        public IEnumerable<UserProfile> GetFollowers(string userId, int skip = 0, int take = 10)
        {

            return Table.Include(e => e.User)
                .Where(x => x.FollowId == userId)
                .Skip(skip).Take(take)
                .OrderBy(x => x.User.UserName)
                .Select(item => userRepo.GetUser(item.UserId));
            
        }

        public IEnumerable<UserProfile> GetFollowing(string userId, int skip = 0, int take = 10)
        {
            return Table.Include(e => e.User)
                .Where(x => x.UserId == userId)
                .Skip(skip).Take(take)
                .OrderBy(x => x.User.UserName)
                .Select(item => userRepo.GetUser(item.FollowId));
        }

        public int CreateFollower(Follow follow)
        {
            Table.Add(follow);
            return this.SaveChanges();
        }

        public int DeleteFollower(Follow follow)
        {
            Table.Remove(follow);
            return this.SaveChanges();
        }

        public int GetFollowingCount(string userId)
        {
            return Table.Count(x => x.UserId == userId);
        }

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