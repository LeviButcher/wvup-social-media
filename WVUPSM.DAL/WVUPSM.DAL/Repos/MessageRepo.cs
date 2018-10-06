using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        private readonly SMContext Db;

        /// <summary>
        ///     Message Table in database
        /// </summary>
        public DbSet<Message> Table;

        /// <summary>
        ///     Database context
        /// </summary>
        public SMContext Context => Db;


        /// <summary>
        ///     Creates Message
        /// </summary>
        /// <param name="message">Message to be created</param>
        public int CreateMessage(Message message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets an individual Message with the passed in MessageId
        /// </summary>
        /// <param name="messageId">Id of the Message to retrieve from database</param>
        /// <returns>A Message viewModel</returns>
        public MessageViewModel GetMessage(int messageId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets all Messages
        /// </summary>
        /// <returns>A list of MessageViewModels</returns>
        public IEnumerable<MessageViewModel> GetMessages()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///     Gets all Messages associated with a User retrieved by userId
        /// </summary>
        ///  <param name="userId">Id of the User</param>
        ///  <param name="skip">the number of Messages to skip. default is 0</param>
        ///  <param name="take">the number of Messages to take, default is 4</param>
        /// <returns>A list of MessageViewModels</returns>
        public IEnumerable<MessageViewModel> GetMessages(int userId, int skip = 0, int take = 4)
        {
            throw new NotImplementedException();
        }
    }
}
