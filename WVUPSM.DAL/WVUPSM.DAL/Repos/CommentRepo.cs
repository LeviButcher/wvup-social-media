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
    /// <summary>
    ///     Comment Repository implementing ICommentRepo
    /// </summary>
    public class CommentRepo : ICommentRepo
    {

        private readonly SMContext _db;

        /// <summary>
        ///     Comment Table in database
        /// </summary>
        public DbSet<Comment> Table;

        /// <summary>
        ///     Database context
        /// </summary>
        public SMContext Context => _db;

        /// <summary>
        ///     Repo Constructor
        /// </summary>
        public CommentRepo()
        {
            _db = new SMContext();
            Table = _db.Set<Comment>();
        }

        /// <summary>
        ///     Overloaded Constructor
        /// </summary>
        protected CommentRepo(DbContextOptions<SMContext> options)
        {
            _db = new SMContext(options);
            Table = _db.Set<Comment>();
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
        ///     Creates comment
        /// </summary>
        /// <param name="comment">Comment to be created</param>
        /// <returns>Number of affected records</returns>
        public int CreateComment(Comment comment)
        {
                Table.Add(comment);
                return this.SaveChanges();
        }

        /// <summary>
        ///     Gets an individual comment with the passed in commentId
        /// </summary>
        /// <param name="commentId">Id of the comment to retrieve from database</param>
        /// <returns>A CommentViewModel</returns>
        public CommentViewModel GetComment(int commentId)
        {
            var comment = Table.Include(x => x.User)
                 .First(x => x.Id == commentId);
            return GetCommentRecord(comment, comment.User);
                
        }

        internal CommentViewModel GetCommentRecord(Comment comment, User user)
        {
            return new CommentViewModel()
            {
                CommentId = comment.Id,
                DateCreated = comment.DateCreated,
                PostId = comment.PostId,
                Text = comment.Text,
                UserId = comment.UserId,
                UserName = user.UserName
            };
        }

        /// <summary>
        ///     Gets all comments
        /// </summary>
        /// <returns>A list of CommentViewModels</returns>
        public IEnumerable<CommentViewModel> GetComments()
        {
            return Table.Include(x => x.User)
                .Select(item => GetCommentRecord(item, item.User));
        }


        /// <summary>
        ///     Gets all comments associated with the Post retrieved by postId
        /// </summary>
        ///  <param name="postId">Id of the Post</param>
        ///  <param name="skip">the number of comments to skip. default is 0</param>
        ///  <param name="take">the number of comments to take, default is 4</param>
        /// <returns>A list of CommentViewModels</returns>
        public IEnumerable<CommentViewModel> GetComments(int postId, int skip = 0, int take = 10 )
        {
            return Table.Include(x => x.User)
                    .Where(x => x.PostId == postId)
                    .Skip(skip).Take(take)
                    .Select(item => GetCommentRecord(item, item.User));
        }
    }
}
