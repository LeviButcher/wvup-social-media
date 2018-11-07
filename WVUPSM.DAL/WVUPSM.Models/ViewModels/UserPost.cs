using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WVUPSM.Models.Services;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    ///     Used for passing a Post to a View
    /// </summary>
    public class UserPost
    {
        /// <summary>
        ///     Id of the Post a being created 
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        ///     Id of User creating Post
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Id of Group a Post is in
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        ///     Email Address of User creating Post
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        ///     UserName of User creating Post
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Text content of Post
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        /// <summary>
        ///     Date Post was created
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///     Time since Post was created
        /// </summary>
        public string TimeSinceCreation
        {
            get
            {
                return Time.TimeSince(DateCreated);
            }
        }

        /// <summary>
        ///    Id of File contained in Post
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        ///    ContentType of File contained in Post
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///    Name of File contained in Post
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///    Count of all Comments for this Post
        /// </summary>
        public int CommentCount { get; set; }
    }
}
