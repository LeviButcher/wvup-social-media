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

        /*
        public async Task<bool> ChangePasswordAsync(User user, string currPass, string newPass)
        {
            
            var result = await UserManager.ChangePasswordAsync(user, currPass, newPass);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
            
            return false;
        }

        public async Task<UserProfile> CreateUserAsync(User user, String password)
        {
           
           var result = await UserManager.CreateAsync(user, password);
           if(result.Succeeded)
           {
               return GetRecord(user, null, null);
           }
           
            return null;
        }
        
        public async Task<bool> DeleteUserAsync(User user)
        {
            
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            
            return false;
        }
        */

        public async Task<User> GetBase(string id)
        {
            return await Table.FindAsync(id);
        }

        public IEnumerable<UserProfile> FindUsers(string term)
        {
            var results = Table
                .Where(e => e.UserName.ToUpper().Contains(term.ToUpper()) || e.Email.ToUpper().Contains(term.ToUpper()));

            List<UserProfile> returnProfiles = new List<UserProfile>();
            foreach(User user in results)
            {
                returnProfiles.Add(GetUser(user.Id));
            }
            return returnProfiles;
        }

        public static UserProfile GetRecord(User user, IEnumerable<Follow> following, IEnumerable<Follow> followers)
            => new UserProfile()
            {
                Email = user.Email,
                UserId = user.Id,
                UserName = user.UserName,
                FollowerCount = followers != null ? followers.Count(): 0,
                FollowingCount = following != null ? following.Count() : 0
            };

        public IEnumerable<UserProfile> GetAllUsers()
        {
            return Table.Include(e => e.Following).Include(e => e.UserFollow)
                .Select(item => GetRecord(item, item.Following, item.UserFollow))
                .OrderBy(x => x.UserName);
        }

        public UserProfile GetUser(string id)
        {
            var user = Table.Include(e => e.Following).Include(e => e.UserFollow)
                .First(x => x.Id == id);

            return user == null ? null : GetRecord(user, user.Following, user.UserFollow);
        }

        //Up in air if method is needed or not
        public UserProfileWithUserPosts GetUserPosts()
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<UserProfile> GetUsers(int skip = 0, int take = 10)
        {
            return Table.Include(e => e.Following).Include(e => e.UserFollow)
                        .Select(item => GetRecord(item, item.Following, item.UserFollow))
                        .Skip(skip).Take(take)
                        .OrderBy(x => x.UserName);
        }

        /*
        public async Task<bool> UpdateUserAsync(User user)
        {
            
            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            
            return false;
        }
        */
    }
}
