using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    ///     Used for CRUD for a Post, neccesary for the IFormFile property
    /// </summary>
    public class CreatePost
    {
        /// <summary>
        ///     Id of created Post
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        ///     Id of User creating Post
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Id of the Group a User is Posting in
        /// </summary>
        public int GroupId { get; set; }

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
        ///     Media content of Post
        /// </summary>
        public IFormFile File { get; set; }
    }
}
