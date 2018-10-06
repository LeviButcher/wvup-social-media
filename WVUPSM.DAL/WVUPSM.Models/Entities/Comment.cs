using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WVUPSM.Models.Entities
{
    /// <summary>
    ///     Represents a Comment within the database.
    /// </summary>
    [Table("Comments", Schema = "SM")]
    public class Comment
    {
        /// <summary>
        ///     Primary key for a Comment
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The User's id that is creating a Comment
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Navigation property to the User creating Comment
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        ///    The Id of the Post being commented on
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        ///     Navigation property to the Post being commented on
        /// </summary>
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        /// <summary>
        ///     Text content for a Comment, max: 1000, min: 1
        /// </summary>
        [MaxLength(1000)]
        [MinLength(1)]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        /// <summary>
        ///     Creation date of this Comment, database seeds this automatically
        /// </summary>
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateCreated { get; set; }


    }
}



 
