using System.ComponentModel.DataAnnotations.Schema;

namespace WVUPSM.Models.Entities
{
    [Table("Follows", Schema = "SM")]
    public class Follow
    {
        //userId == Following
        public string UserId { get; set; }        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        //followId == Followers
        public string FollowId { get; set; }
        [ForeignKey(nameof(FollowId))]
        public User Following { get; set; }
    }
}
