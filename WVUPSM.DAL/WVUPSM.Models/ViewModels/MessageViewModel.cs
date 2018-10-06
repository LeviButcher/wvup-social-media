using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        public string UserId { get; set; }

        /// <summary>
        ///     UserName of First User creating Message
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Id of Second User in Message
        /// </summary>
        public string OtherUserId { get; set; }

        /// <summary>
        ///     UserName of Second User creating Message
        /// </summary>
        public string OtherUserName { get; set; }

        /// <summary>
        ///     Text content of Message
        /// </summary>
        [DataType(DataType.MultilineText)]
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
                var now = DateTime.Now;
                var difference = now - DateCreated;
                if (difference.Days > 0)
                {
                    return $"{difference.Days} days ago";
                }
                if (difference.Hours > 0)
                {
                    return $"{difference.Hours} hours ago";
                }
                else if (difference.Minutes > 0)
                {
                    return $"{difference.Minutes} minutes ago";
                }
                else if (difference.Seconds > 0)
                {
                    return $"{difference.Seconds} seconds ago";
                }
                else if (difference.Seconds < 0)
                {
                    return $"{difference.Hours} seconds in the future???";
                }
                else
                {
                    return "Just now";
                }
            }
        }
    }
}