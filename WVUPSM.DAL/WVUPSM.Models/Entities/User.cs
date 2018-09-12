using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WVUPSM.Models.Entities
{
    [Table("Users", Schema = "SM")]
    public class User : IdentityUser
    {
        //comments in form of User

        //Following is people I'm following
        //userid == myId is people I'm following
        [InverseProperty(nameof(Follow.User))]
        public List<Follow> Following { get; set; } = new List<Follow>();

        //UserFollow is people following me
        //followId == myId is people following me
        [InverseProperty(nameof(Follow.Person))]
        public List<Follow> Followers { get; set; } = new List<Follow>();

        [InverseProperty(nameof(Post.User))]
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
