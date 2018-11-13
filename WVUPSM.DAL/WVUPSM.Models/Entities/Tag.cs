using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WVUPSM.Models.Entities
{

    /// <summary>
    ///    Tags Table in Database
    /// </summary>
    [Table("Tags", Schema = "SM")]
    public class Tag
    {
        /// <summary>
        /// The Tag's Id 
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        ///     List of UserTags for this Tag
        /// </summary>
        [InverseProperty(nameof(UserTag.Tag))]
        public List<UserTag> UserTags { get; set; } = new List<UserTag>();


    }
}
