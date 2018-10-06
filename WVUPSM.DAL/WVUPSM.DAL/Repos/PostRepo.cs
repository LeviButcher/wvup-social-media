using Microsoft.AspNetCore.Identity;
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos
{
    /// <summary>
    ///     Post Repository implementing IPostRepo
    /// </summary>
    public class PostRepo : IPostRepo
    {
        private readonly SMContext _db;

        /// <summary>
        ///     Follow Repo
        /// </summary>
        private FollowRepo _followRepo;
        /// <summary>
        ///     User Repo
        /// </summary>
        private UserRepo _userRepo;

        /// <summary>
        ///     Post Table in database
        /// </summary>
        public DbSet<Post> Table;

        /// <summary>
        ///     Database context
        /// </summary>
        public SMContext Context => _db;

        /// <summary>
        ///     Repo Constructor
        /// </summary>
        public PostRepo()
        {
            _db = new SMContext();
            Table = _db.Set<Post>();
            _userRepo = new UserRepo();
            _followRepo = new FollowRepo();
        }

        /// <summary>
        ///     Overloaded Constructor
        /// </summary>
        /// <param name="options">DbContextOptions</param>
        protected PostRepo(DbContextOptions<SMContext> options)
        {
            _db = new SMContext(options);
            Table = _db.Set<Post>();
            _userRepo = new UserRepo();
            _followRepo = new FollowRepo();
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
        {
            UserPost userPost = new UserPost()
            {
                DateCreated = post.DateCreated,
                PostId = post.Id,
                Text = post.Text,
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FilePath = post.FilePath,
                IsPicture = post.IsPicture,
                FileName = post.FileName
            };

            return userPost;
        }
            
        public UserPost GetPost(int id)
        {
            return Table.Include(e => e.User)
               .Where(x => x.Id == id)
               .Select(item => GetRecord(item, item.User)).First();
        }

        public Post GetBasePost(int id)
            => Table.First(x => x.Id == id);


        /*
         * Stack overflow that helped me write this - https://stackoverflow.com/questions/2767709/join-where-with-linq-and-lambda?rq=1
         * 
         * Join the post and follow table on primary and foreign key then select all Follows that have the userid of the users
         */
        public IEnumerable<UserPost> GetFollowingPosts(string userId, int skip = 0, int take = 10)
        {
            return Table.Include(x => x.User).ThenInclude(x => x.Followers)
                .Join(Context.Follows,
                post => post.UserId,
                x => x.FollowId,
                (post, follow) => new { Post = post, Follow = follow })
                .Where(x => x.Follow.UserId == userId)
                .OrderByDescending(x => x.Post.DateCreated)
                .Skip(skip).Take(take)
                .Select(item => GetRecord(item.Post, item.Post.User));
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
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateCreated)
                .Skip(skip).Take(take)
                .Select(item => GetRecord(item, item.User));
        }

        public IEnumerable<UserPost> GetGroupPost(int groupId, int skip = 0, int take = 10)
        {
            return Table.Include(x => x.User)
                 .Where(x => x.GroupId == groupId)
                 .Skip(skip).Take(take)
                 .OrderByDescending(x => x.DateCreated)
                 .Select(item => GetRecord(item, item.User));
        }
    }
}
