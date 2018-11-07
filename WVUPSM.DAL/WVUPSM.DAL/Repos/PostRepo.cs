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
    ///     Post Repository for SQL Server
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
        ///     Comment Repo
        /// </summary>
        private CommentRepo _commentRepo;

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
         //   _userRepo = new UserRepo();
          //  _followRepo = new FollowRepo();
         //   _commentRepo = new CommentRepo();
        }

        /// <summary>
        ///     Overloaded Constructor
        /// </summary>
        /// <param name="options">DbContextOptions</param>
        public PostRepo(DbContextOptions<SMContext> options)
        {
            _db = new SMContext(options);
            Table = _db.Set<Post>();
            _userRepo = new UserRepo(options);
            _followRepo = new FollowRepo(options);
            _commentRepo = new CommentRepo(options);

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
                FileName = post.File.FileName,
                ContentType = post.File.ContentType,
                FileId = post.File.Id,
                CommentCount = post.Comments.Count()
            };

            return userPost;
        }

        /// <summary>
        ///     Gets a UserPost matching the id
        /// </summary>
        /// <param name="id">post's id</param>
        /// <returns>Returns UserPost viewmodel matching that id</returns>
        public UserPost GetPost(int id)
        {
            return Table.Include(e => e.User).Include(e => e.Comments)
               .Where(x => x.Id == id)
               .Select(item => GetRecord(item, item.User)).First();
        }

        /// <summary>
        ///     Gets a base post matching the id
        /// </summary>
        /// <param name="id">post's id</param>
        /// <returns>Returns post matching that id</returns>
        public Post GetBasePost(int id)
            => Table.First(x => x.Id == id);

        /// <summary>
        ///     Gets post's from the people that this user is following
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <param name="skip">Amount of records to skip</param>
        /// <param name="take">Amount of record to take</param>
        /// <returns>amount of UserPost less then or equal to take</returns>
        //  Stack overflow that helped me write this - https://stackoverflow.com/questions/2767709/join-where-with-linq-and-lambda?rq=1
        public IEnumerable<UserPost> GetFollowingPosts(string userId, int skip = 0, int take = 10)
        {
            return Table.Include(x => x.User).ThenInclude(x => x.Followers).Include(e => e.Comments)
                .Join(Context.Follows,
                post => post.UserId,
                x => x.FollowId,
                (post, follow) => new { Post = post, Follow = follow })
                .Where(x => x.Follow.UserId == userId)
                .OrderByDescending(x => x.Post.DateCreated)
                .Skip(skip).Take(take)
                .Select(item => GetRecord(item.Post, item.Post.User));
        }


        /// <summary>
        ///     Deletes the post provided from the Database
        /// </summary>
        /// <param name="post">Post that must have a id and timestamp</param>
        /// <returns>number of records effected</returns>
        public int DeletePost(Post post)
        {
             Table.Remove(post);
             return this.SaveChanges();
        }

        /// <summary>
        ///     Creates a new post in the database
        /// </summary>
        /// <param name="post">A new post without a id</param>
        /// <returns>number of records created</returns>
        public int CreatePost(Post post)
        {
            Table.Add(post);
            return this.SaveChanges();
        }

        /// <summary>
        ///     Gets the user's post matching the userId provided
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <param name="skip">Records to skip</param>
        /// <param name="take">Recrods to take</param>
        /// <returns>Amount of UserPost less then or equal to take</returns>
        public IEnumerable<UserPost> GetUsersPost(string userId, int skip = 0, int take = 10)
        {
           return Table.Include(x => x.User).Include(e => e.Comments)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateCreated)
                .Skip(skip).Take(take)
                .Select(item => GetRecord(item, item.User));
        }

        /// <summary>
        ///     Gets the posts made in Group matching groupId provided
        /// </summary>
        /// <param name="groupId">user's id</param>
        /// <param name="skip">Records to skip</param>
        /// <param name="take">Recrods to take</param>
        /// <returns>Amount of UserPost less then or equal to take</returns>
        public IEnumerable<UserPost> GetGroupPost(int groupId, int skip = 0, int take = 10)
        {
            return Table.Include(x => x.User).Include(e => e.Comments)
                 .Where(x => x.GroupId == groupId)
                 .OrderByDescending(x => x.DateCreated)
                 .Skip(skip).Take(take)
                 .Select(item => GetRecord(item, item.User));
        }

    }
}
