using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WVUPSM.Models.Services;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    ///     Used for passing a Comment to a View
    /// </summary>
    public class CommentViewModel
    {
        /// <summary>
        ///     Id of created Comment
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        ///     Id of User creating Comment
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Id of the Post a User is Commenting on
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        ///     UserName of User creating Comment
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Text content of Comment
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        /// <summary>
        ///     Date Comment was created
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///     Time since Comment was created
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



