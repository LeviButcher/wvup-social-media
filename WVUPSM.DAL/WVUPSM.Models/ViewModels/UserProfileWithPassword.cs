using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    public class UserProfileWithPassword : UserProfileWithRole
    {
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
