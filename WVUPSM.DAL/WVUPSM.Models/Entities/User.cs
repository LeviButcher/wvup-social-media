using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WVUPSM.Models.Entities
{
    [Table("Users", Schema = "SM")]
    public class User : IdentityUser
    {
        [InverseProperty(nameof(Follow.Following))]
        public List<Follow> Following { get; set; } = new List<Follow>();

        [InverseProperty(nameof(Post.User))]
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
