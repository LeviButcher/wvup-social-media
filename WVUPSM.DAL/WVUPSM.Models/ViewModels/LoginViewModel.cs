using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    ///     Used for passing a Login info to a View
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        ///     User's Email
        /// </summary>
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        /// <summary>
        ///     User's Password
        /// </summary>
        [DataType(DataType.Password)]
        [Required]
        public string password { get; set; }
    }
}
