using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WVUPSM.Models.Entities
{
    /// <summary>
    ///     Represents a Post within the database.
    /// </summary>
    [Table("Posts", Schema = "SM")]
    public class Post
    {
        /// <summary>
        ///     Primary key for a post
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        ///     Text content for a post, max: 4000, min: 1
        /// </summary>
        [MaxLength(4000)]
        [MinLength(1)]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        /// <summary>
        ///     Creation data of this post, database seeds this automatically
        /// </summary>
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///     Foriegn key to User table <see cref="User"/>
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        ///     Navigation property to the User table <see cref="User"/>
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        ///     Concurrency check for the Post table
        /// </summary>
        [Timestamp]
        public byte[] Timestamp { get; set; }

        /// <summary>
        ///     Path to the picture associated with this post on the server
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///     Wether or not the File attached is a Picture
        /// </summary>
        public bool IsPicture { get; set; }

        /// <summary>
        ///     Name of the file attached to the Post
        /// </summary>
        public string FileName { get; set; }
    }
}
