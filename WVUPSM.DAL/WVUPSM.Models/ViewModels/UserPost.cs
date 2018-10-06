﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        /// <summary>
        ///    Path of File contained in Post
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///    Boolean, true if File is an image, else false
        /// </summary>
        public bool IsPicture { get; set; }

        /// <summary>
        ///    Name of File contained in Post
        /// </summary>
        public string FileName { get; set; }
    }
}
