using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos.Interfaces
{
    /// <summary>
    ///     Message Repository Database API
    /// </summary>
    public interface IMessageRepo 
    {
        /// <summary>
        ///     Gets a Message based on which Id is passed in.
        /// </summary>
        /// <param name="messageId">The id of this Message</param>
        /// <returns>MessageViewModel for Message with id of messageId</returns>
        MessageViewModel GetMessage(int messageId);

        /// <summary>
        ///     Creates a new Message record
        /// </summary>
        /// <param name="message">Message object that MUST contain a userId and an otherUserId</param>
        /// <returns>integer value of number of records affected</returns>
        int CreateMessage(Message message);

        /// <summary>
        ///    Gets all Messages
        /// </summary>        
        /// <returns>An IEnumerable of all Messages</returns>
        IEnumerable<MessageViewModel> GetMessages();

        /// <summary>
        ///    Gets all Messages for a User, using skip and take
        /// </summary>        
        /// <returns>An IEnumerable of all Messages for a User</returns>
        IEnumerable<MessageViewModel> GetMessages(int userId, int skip, int take);

    }
}
