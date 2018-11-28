using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WVUPSM.Models.Entities
{
    /// <summary>
    ///    UserTag Table in Database
    /// </summary>
    [Table("UserTags", Schema = "SM")]
    public class UserTag
    {
        /// <summary>
        ///    Id of the User
        /// </summary>
        [Required]
        public string UserId { get; set; }
        /// <summary>
        ///     Navigation property to the User table <see cref="User"/>
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        ///     Id of the Group the User is in
        /// </summary>
        [Required]
        public int TagId { get; set; }

        /// <summary>
        ///     Navigation property to the Group table <see cref="Tag"/>
        /// </summary>
        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; }
    }
}
