using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WVUPSM.Models.Services;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    ///     Used for passing a Message to a View
    /// </summary>
    public class MessageViewModel
    {
        /// <summary>
        ///     Id of created Message
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        ///     Id of First User in Message
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        ///     UserName of First User creating Message
        /// </summary>
        public string SenderUserName { get; set; }

        /// <summary>
        ///     Id of Second User in Message
        /// </summary>
        public string RecieverId { get; set; }

        /// <summary>
        ///     UserName of Second User creating Message
        /// </summary>
        public string RecieverUserName { get; set; }

        /// <summary>
        ///     Text content of Message
        /// </summary>
        [DataType(DataType.MultilineText)]
        [MaxLength(300)]
        [MinLength(1)]
        [Required]
        public string Text { get; set; }

        /// <summary>
        ///     Date Message was created
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///     Time since Message was created
        /// </summary>
        public string TimeSinceCreation
        {
            get
            {
                return Time.TimeSince(DateCreated);
            }
        }
    }
}