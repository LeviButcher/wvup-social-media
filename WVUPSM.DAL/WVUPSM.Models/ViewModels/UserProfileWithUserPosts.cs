using System;
using System.Collections.Generic;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    public class UserProfileWithUserPosts : UserProfile
    {
        public IEnumerable<UserPost> Posts { get; set; }
    }
}
