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

namespace WVUPSM.DAL.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly SMContext Db;
        private DbSet<User> Table;
        public SMContext Context => Db;

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

        public bool ChangePassword(string userId, string currPass, string newPass)
        {
            throw new NotImplementedException();
        }

        public UserProfile CreateUser(User user)
        {
            
        }

        public int DeleteUser(User user)
        {
            Table.Remove(user);
            return this.SaveChanges();
        }

        public IEnumerable<UserProfile> FindUsers(string term)
        {
            return Table.Include(e => e.Following).Include(e => e.UserFollow)
                .Where(e => e.UserName.Contains(term))
                .Select(item => GetRecord(item, item.Following, item.UserFollow))
                .OrderBy(x=> x.UserName);
        }

        public static UserProfile GetRecord(User user, IEnumerable<Follow> following, IEnumerable<Follow> followers)
            => new UserProfile()
            {
                Email = user.Email,
                UserId = user.Id,
                UserName = user.UserName,
                FollowerCount = followers.Count(),
                FollowingCount = following.Count()
            };

        public IEnumerable<UserProfile> GetAllUsers()
        {
            return Table.Include(e => e.Following).Include(e => e.UserFollow)
                .Select(item => GetRecord(item, item.Following, item.UserFollow))
                .OrderBy(x => x.UserName);
        }

        public UserProfile GetUser(string id)
        {
            return Table.Include(e => e.Following).Include(e => e.UserFollow)
                .Where(x => x.Id == id)
                .Select(item => GetRecord(item, item.Following, item.UserFollow)).First();
        }

        public UserProfileWithUserPosts GetUserPosts()
        {
            throw new NotImplementedException();
        }
        /*
        public static createUserProfileWithPosts(UserProfile user, IEnumerable<UserPost> posts)
            => new UserProfileWithUserPosts
            {
                Email = ,
                UserId = ,
                UserName = ,
                FollowingCount = ,
                FollowerCount = ,
                Posts = 
            }
        */
        public IEnumerable<UserProfile> GetUsers(int skip = 0, int take = 10)
        {
            return Table.Include(e => e.Following).Include(e => e.UserFollow)
                        .Select(item => GetRecord(item, item.Following, item.UserFollow))
                        .Skip(skip).Take(take)
                        .OrderBy(x => x.UserName);
        }

        public UserProfile UpdateUser(User user)
        {
            Table.Update(user);
            SaveChanges();
            return GetUser(user.Id);
        }
    }
}
