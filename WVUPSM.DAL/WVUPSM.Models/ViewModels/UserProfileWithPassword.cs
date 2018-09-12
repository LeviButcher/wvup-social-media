using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    public class UserProfileWithPassword : UserProfile
    {
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
