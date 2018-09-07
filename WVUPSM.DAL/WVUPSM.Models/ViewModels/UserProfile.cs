using System;
using System.Collections.Generic;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    public class UserProfile
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public int FollowerCount { get; set; }

        public int FollowingCount { get; set; }
    }
}
