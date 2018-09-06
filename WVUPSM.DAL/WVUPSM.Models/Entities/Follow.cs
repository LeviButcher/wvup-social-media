using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WVUPSM.Models.Entities
{
    [Table("Follows", Schema = "SM")]
    public class Follow
    {        
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }


        public Guid FollowId { get; set; }
        [ForeignKey(nameof(FollowId))]
        public User Following { get; set; }
    }
}
