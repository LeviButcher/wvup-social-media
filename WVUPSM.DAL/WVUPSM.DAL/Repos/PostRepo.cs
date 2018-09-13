using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos
{
    public class PostRepo : IPostRepo
    {
        private readonly SMContext Db;
        private FollowRepo followRepo;
        private UserRepo userRepo;
        public DbSet<Post> Table;
        public SMContext Context => Db;
        
        public PostRepo()
        {
            Db = new SMContext();
            Table = Db.Set<Post>();
            userRepo = new UserRepo();
            followRepo = new FollowRepo();
    }

        protected PostRepo(DbContextOptions<SMContext> options)
        {
            Db = new SMContext(options);
            Table = Db.Set<Post>();
            userRepo = new UserRepo();
            followRepo = new FollowRepo();
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

        public static UserPost GetRecord(Post post, User user)
            => new UserPost()
            {
                DateCreated = post.DateCreated,
                PostId = post.Id,
                Text = post.Text,
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName
            };


        public UserPost GetPost(int id)
        {
            return Table.Include(e => e.User)
               .Where(x => x.Id == id)
               .Select(item => GetRecord(item, item.User)).First();
        }

        public Post GetBasePost(int id)
            => Table.First(x => x.Id == id);
       

        //TODO: change to join
        public IEnumerable<UserPost> GetFollowersPosts(string userId, int skip = 0, int take = 10)
        {
            IEnumerable<UserProfile> followers = new List<UserProfile>();
            List<UserPost> followPosts = new List<UserPost>();
            
            followers = followRepo.GetFollowing(userId, skip, followRepo.GetFollowerCount(userId));
            for(int i = 0; i < followers.Count(); i++)
            {
                int totalPost = Table.Count(x => x.UserId == followers.ElementAt(i).UserId);
                followPosts.AddRange(GetUsersPost(followers.ElementAt(i).UserId, 0, totalPost));
            }

            return followPosts.OrderByDescending(x => x.DateCreated);

        }

        public int DeletePost(Post post)
        {
             Table.Remove(post);
             return this.SaveChanges();
        }

        public int CreatePost(Post post)
        {
            Table.Add(post);
            return this.SaveChanges();
            

        }

        public IEnumerable<UserPost> GetUsersPost(string userId, int skip = 0, int take = 10)
        {
           return Table.Include(x => x.User)
                .Where(x => x.UserId == userId).Skip(skip).Take(take)
                .OrderByDescending(x => x.DateCreated)
                .Select(item => GetRecord(item, item.User));
        }
    }
}
