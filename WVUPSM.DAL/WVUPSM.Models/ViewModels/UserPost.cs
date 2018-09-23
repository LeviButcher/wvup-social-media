using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    public class UserPost
    {
        public int PostId { get; set; }

        public string UserId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public string FilePath { get; set; }

        public bool IsPicture { get; set; }

        public string FileName { get; set; }
    }
}
