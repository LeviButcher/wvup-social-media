using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WVUPSM.Models.Entities
{
    [Table("Posts", Schema = "SM")]
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(4000)]
        [MinLength(1)]
        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
