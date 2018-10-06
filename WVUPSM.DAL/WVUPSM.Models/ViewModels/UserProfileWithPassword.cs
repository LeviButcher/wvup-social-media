using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    ///     Used for Confirming User's Password
    /// </summary>
    public class UserProfileWithPassword : UserProfileWithRole
    {
        /// <summary>
        ///     User's Password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        ///     User's Re-entered Password
        /// </summary>
        [Required]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
