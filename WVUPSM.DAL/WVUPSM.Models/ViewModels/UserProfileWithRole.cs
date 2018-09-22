using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WVUPSM.Models.ViewModels
{
    public class UserProfileWithRole
    {
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string RoleId { get; set; }
    }
}
