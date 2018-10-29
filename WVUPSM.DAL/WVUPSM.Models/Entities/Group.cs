using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WVUPSM.Models.Entities
{
    /// <summary>
    ///     Represents the Group table within the database
    /// </summary>
    [Table("Groups", Schema = "SM")]
    public class Group
    {
        /// <summary>
        /// The Groups Id 
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        ///     Id of User creating Group
        /// </summary>
        [Required]
        public string OwnerId { get; set; }

        /// <summary>
        ///     Navigation property to the User table <see cref="User"/>
        /// </summary>
        [ForeignKey(nameof(OwnerId))]
        public User User { get; set; }

        /// <summary>
        ///     Group Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Group Bio
        /// </summary>
        [MaxLength(4000)]
        [MinLength(1)]
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        /// <summary>
        ///     Date Group was created
        /// </summary>
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///     List of Users in Group
        /// </summary>
        [InverseProperty(nameof(UserGroup.Group))]
        public List<UserGroup> Members { get; set; } = new List<UserGroup>();
    }
}
