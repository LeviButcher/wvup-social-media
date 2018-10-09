using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WVUPSM.Models.Services;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    /// Represnts a Message shown in a Inbox view
    /// </summary>
    public class InboxMessageViewModel
    {
        /// <summary>
        /// ID of the user who you (the user) are messaging
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Username of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Last message sent in the conversation by them or you
        /// </summary>
        [DataType(DataType.MultilineText)]
        [MaxLength(300)]
        [MinLength(1)]
        [Required]
        public string LastMessage { get; set; }

        /// <summary>
        ///     DateTime created
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public string TimeSinceCreation
        {
            get
            {
                return Time.TimeSince(DateCreated);
            }
        }
    }
}
