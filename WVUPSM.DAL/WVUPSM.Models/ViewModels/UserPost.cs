using System;
using System.Collections.Generic;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    public class UserPost
    {
        public int PostId { get; set; }

        public string UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Text { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
