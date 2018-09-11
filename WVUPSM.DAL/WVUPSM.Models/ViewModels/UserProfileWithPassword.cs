using System;
using System.Collections.Generic;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    public class UserProfileWithPassword : UserProfile
    {
        public string password { get; set; }
    }
}
