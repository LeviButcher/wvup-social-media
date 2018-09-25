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
        public int PostId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public IFormFile File { get; set; }
    }
}
