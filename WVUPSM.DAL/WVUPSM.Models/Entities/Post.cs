﻿using System;
using System.Collections.Generic;
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
        ///     Foriegn key to File table <see cref="File"/>
        /// </summary>
        public int? FileId { get; set; }

        /// <summary>
        ///     Navigation property to the File table <see cref="File"/>
        /// </summary>
        [ForeignKey(nameof(FileId))]
        public File File { get; set; }
        

        /// <summary>
        ///     If Post is a post in a group, the Id of that Group, else null
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        ///     Navigation property to the Group table <see cref="Group"/>
        /// </summary>
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }

        /// <summary>
        ///     Concurrency check for the Post table
        /// </summary>
        [Timestamp]
        public byte[] Timestamp { get; set; }

        /// <summary>
        ///     All comments on this post.
        /// </summary>
        [InverseProperty(nameof(Comment.Post))]
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
