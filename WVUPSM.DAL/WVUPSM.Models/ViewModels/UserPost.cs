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

        public int GroupId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public string TimeSinceCreation
        {
            get
            {
                var now = DateTime.Now;
                var difference = now - DateCreated;
                if(difference.Days > 0)
                {
                    return $"{difference.Days} days ago";
                }
                if (difference.Hours > 0)
                {
                    return $"{difference.Hours} hours ago";
                }
                else if(difference.Minutes > 0)
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

        public string FilePath { get; set; }

        public bool IsPicture { get; set; }

        public string FileName { get; set; }
    }
}
