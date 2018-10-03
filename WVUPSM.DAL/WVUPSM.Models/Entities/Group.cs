using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WVUPSM.Models.Entities
{
    [Table("Groups", Schema = "SM")]
    public class Group
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public User User { get; set; }

        public string Name { get; set; }

        [MaxLength(4000)]
        [MinLength(1)]
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateCreated { get; set; }

        [InverseProperty(nameof(UserGroup.Group))]
        public List<UserGroup> Members { get; set; } = new List<UserGroup>();
    }
}
