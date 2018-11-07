using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    ///     Represents a User's Profile for user in the View
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        ///     User's database Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Email address of the user
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        ///     User's userName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     User's Bio
        /// </summary>
        [MaxLength(400)]
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        /// <summary>
        ///     Number of people following this user
        /// </summary>
        public int FollowerCount { get; set; }

        /// <summary>
        ///     Number of people this user is following
        /// </summary>
        public int FollowingCount { get; set; }

        /// <summary>
        ///    Id of File associated with User's ProfilePicture 
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        ///    ContentType of File associated with User's ProfilePicture 
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///    Name of File associated with User's ProfilePicture
        /// </summary>
        public string FileName { get; set; }
    }
}
