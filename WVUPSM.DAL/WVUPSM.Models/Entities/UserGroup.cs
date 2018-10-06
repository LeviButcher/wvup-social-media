using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WVUPSM.Models.Entities
{
    /// <summary>
    ///    UserGroup Table in Database
    /// </summary>
    [Table("UserGroups", Schema = "SM")]
    public class UserGroup
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
        public int GroupId { get; set; }

        /// <summary>
        ///     Navigation property to the Group table <see cref="Group"/>
        /// </summary>
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }
    }
}
