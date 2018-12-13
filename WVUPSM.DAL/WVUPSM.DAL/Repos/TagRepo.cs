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
    ///     Tag Respository for SQL Server implemenation
    /// </summary>
    public class TagRepo : ITagRepo
    {
        private readonly SMContext Db;

        /// <summary>
        ///  Tag table in Db
        /// </summary>
        public DbSet<Tag> Table;

        /// <summary>
        ///    UserTag table in Db
        /// </summary>
        public DbSet<UserTag> UserTagTable;

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public TagRepo()
        {
            Db = new SMContext();
            Table = Db.Set<Tag>();
            UserTagTable = Db.Set<UserTag>();
        }

        /// <summary>
        ///     Overloaded Constructor, used by dependcy injection when a connection string is provided
        /// </summary>
        /// <param name="options"></param>
        public TagRepo(DbContextOptions<SMContext> options)
        {
            Db = new SMContext(options);
            Table = Db.Set<Tag>();
            UserTagTable = Db.Set<UserTag>();
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

        /// <summary>
        ///     Saves changes to DB
        /// </summary>
        /// <returns>1 if successful, 0 if not</returns>
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

        /// <summary>
        ///     If Tag name is not found in Db, it is created and then a UserTag object 
        ///     is created with tag.Id and userId
        ///     Else If, if a UserTag object doesn't exist with tag.Id and userId, one is created,
        ///     Else, returns 0
        /// <param name="tag"></param>
        /// <param name="userId"></param>
        /// <returns>Result of saveChanges, or zero if nothing affected</returns>
        public int CreateTag(string name, string userId)
        {   
            if(!IsTag(name.ToLower()))
            {
               
                Table.Add(new Tag
                {
                    Name = name.ToLower()
                });
                SaveChanges();
                Tag tag = GetTagByName(name);

                UserTagTable.Add(new UserTag
                {
                    TagId = tag.Id,
                    UserId = userId
                });

                return this.SaveChanges();
            }
            else if(!IsUserTag(GetTagByName(name).Id, userId))
            {
                Tag tag = GetTagByName(name.ToLower());
                UserTagTable.Add(new UserTag
                {
                    TagId = tag.Id,
                    UserId = userId
                });

                return this.SaveChanges();
            }
            else
            {
                return 0;
            }
            
        }

        /// <summary>
        ///     Returns a tag with matching name
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private Tag GetTagByName(string word)
        {
            return Table.Where(x => x.Name.ToLower() == word.ToLower()).FirstOrDefault();
        }

        /// <summary>
        ///    Returns a tag with matching Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Tag GetTagById(int id)
        {
            return Table.Where(x => x.Id == id).FirstOrDefault();
        }

        /// <summary>
        ///  Deletes tag from database
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int DeleteTag(Tag tag)
        {
            Table.Remove(tag);

            return this.SaveChanges();
        }

        /// <summary>
        ///  Removes tag when user deletes it from their interests
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int RemoveUserTag(int tagId, string userId)
        {
            if(IsUserTag(tagId, userId))
            {
                UserTag ut = UserTagTable.Where(x => x.TagId == tagId && x.UserId == userId).FirstOrDefault();
                UserTagTable.Remove(ut);
            }

            return this.SaveChanges();
        }

        /// <summary>
        ///     Gets all UserTags with argument TagId 
        /// </summary>
        /// <param name="tagId">Argument to search</param>
        /// <returns></returns>
        public IEnumerable<UserTag> GetUserTagsByTag(int tagId)
        {
            return UserTagTable.Where(x => x.TagId == tagId).AsEnumerable();
        }

        /// <summary>
        ///     Gets all UserTags with argument UserId 
        /// </summary>
        /// <param name="userId">Argument to search</param>
        /// <returns></returns>
        public IEnumerable<UserTag> GetUserTagsByUser(string userId)
        {
            return UserTagTable.Where(x => x.UserId== userId).AsEnumerable();
        }

        /// <summary>
        ///     Searches db for string value of Tag
        /// </summary>
        /// <param name="word">String value to search</param>
        /// <returns></returns>
        public bool IsTag(string word)
        {
            return Table.Any(x => x.Name == word.ToLower());
        }

        /// <summary>
        ///     Checks DB for a UserTag object with matching tag string value and userId
        /// </summary>
        /// <param name="tagId">Gets tag from Db to compare string value</param>
        /// <param name="userId">users's Id</param>
        /// <returns></returns>
        public bool IsUserTag(int tagId, string userId)
        {
            Tag tag = GetTagById(tagId);

            if(UserTagTable.Any(x => x.Tag.Name == tag.Name && x.UserId == userId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///     Creates multiple tags and assocates them to a user
        /// </summary>
        /// <param name="spaceDelimitedTags"></param>
        /// <param name="userId"></param>
        /// <returns>1 if successful, 0 otherwise</returns>
        public int CreateTags(string spaceDelimitedTags, string userId)
        {
            var tags = spaceDelimitedTags.Split(' ');
            if(tags.Length > 0)
            {
                foreach(var tag in tags)
                {
                    CreateTag(tag, userId);
                }
            }
            return 1;
        }

        /// <summary>
        ///     Removes all tags assocatiated with this user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>1 if successful, 0 otherwise</returns>
        public int DropAllUserTags(string userId)
        {
            UserTagTable.RemoveRange(UserTagTable.Where(x => x.UserId == userId));
            return this.SaveChanges();
        }

        /// <summary>
        ///   Returns a list of all tags in Db matching search term
        /// </summary>
        /// <param name="term">term to be searched</param>
        /// <returns>A list of all Tags</returns>
        public IEnumerable<Tag> FindTags(string term)
        {
            var results = Table.Where(e => e.Name.ToUpper().Contains(term.ToUpper()));
            List<Tag> foundTags = new List<Tag>();

            foreach (Tag tag in results)
            {
                foundTags.Add(tag);
            }
            return foundTags;
        }

        /// <summary>
        ///   Returns a list of all Users in Db with UserTag that matches search term
        /// </summary>
        /// <param name="term">term to be searched</param>
        /// <returns>A list of all Tags</returns>
        public IEnumerable<UserTag> GetUsersByTagName(string term)
        {
            List<UserTag> userTags = null;
            var tags = FindTags(term);
            if(tags != null)
            {
                foreach(Tag tag in tags)
                {
                    userTags = GetUserTagsByTag(tag.Id).ToList();
                }
            }
            Console.WriteLine("tags count is " + tags.Count());
                      
            return userTags;

        }
    }
}
