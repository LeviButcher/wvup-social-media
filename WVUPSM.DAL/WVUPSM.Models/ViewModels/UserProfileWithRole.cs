using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    ///     Used for Passsing Profile With Role
    /// </summary>
    public class UserProfileWithRole
    {
        /// <summary>
        ///     User's Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     User's Username
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        /// <summary>
        ///     User's Email
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        ///     Id of Role this User is in
        /// </summary>
        [Required]
        public string RoleId { get; set; }
    }
}
