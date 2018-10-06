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
    ///     Message Repository implementing IMessageRepo
    /// </summary>
    public class MessageRepo : IMessageRepo
    {

        private readonly SMContext _db;

        /// <summary>
        ///     Message Table in database
        /// </summary>
        public DbSet<Message> Table;

        /// <summary>
        ///     Database context
        /// </summary>
        public SMContext Context => _db;

        /// <summary>
        ///     Repo Constructor
        /// </summary>
        public MessageRepo()
        {
            _db = new SMContext();
            Table = _db.Set<Message>();
        }

        /// <summary>
        ///     Overloaded Constructor
        /// </summary>
        protected MessageRepo(DbContextOptions<SMContext> options)
        {
            _db = new SMContext(options);
            Table = _db.Set<Message>();
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
        ///     Creates Message
        /// </summary>
        /// <param name="message">Message to be created</param>
        public int CreateMessage(Message message)
        {
            Table.Add(message);
            return this.SaveChanges();
        }

        /// <summary>
        ///     Gets an individual Message
        /// </summary>
        /// <param name="messageId">Id of the Message to retrieve from database</param>
        /// <returns>A Message viewModel</returns>
        public MessageViewModel GetMessage(int messageId)
        {
            
          return  Table.Include(x => x.User)
                .Where(x => x.Id == messageId)
                .Select(item => GetRecord(item)).First();
           
        }

        /// <summary>
        ///     Gets an individual Message
        /// </summary>
        /// <param name="message"> Message to retrieve from database</param>
        /// <returns>A Message viewModel</returns>
        internal MessageViewModel GetRecord(Message message)
        {
            return new MessageViewModel()
            {
                MessageId = message.Id,
                DateCreated = message.DateCreated,
                Text = message.Text,
                UserId = message.UserId,
                UserName = message.User.UserName
            };
        }

        /// <summary>
        ///     Gets all Messages
        ///     NotImplemented
        /// </summary>
        /// <returns>A list of MessageViewModels</returns>
        public IEnumerable<MessageViewModel> GetMessages()
        {
            return Table.Include(x => x.User)
               .Select(item => GetRecord(item));
        }

        /// <summary>
        ///     Gets all Conversations
        /// </summary>
        /// <returns>A list of MessageViewModels</returns>
        public IEnumerable<MessageViewModel> GetConversations(string userId, int skip = 0, int take = 20)
        {
            return Table.Include(x => x.User)
                   .Where(x => x.UserId == userId)
                   .Skip(skip).Take(take)
                   .Select(item => GetRecord(item));
        }


        /// <summary>
        ///     Gets conversation between two users
        /// </summary>
        ///  <param name="userId">Id of the first User in converation</param>
        ///  <param name="otherUserId">Id of the Other User in conversation</param>
        ///  <param name="skip">the number of Messages to skip. default is 0</param>
        ///  <param name="take">the number of Messages to take, default is 4</param>
        /// <returns>A list of MessageViewModels</returns>
        public IEnumerable<MessageViewModel> GetConversation(string userId, string otherUserId, int skip = 0, int take = 20)
        {
            return Table.Include(x => x.User)
                   .Where(x => x.UserId == userId && x.OtherUserId == otherUserId)
                   .Skip(skip).Take(take)
                   .Select(item => GetRecord(item)); 
        }
    }
}
