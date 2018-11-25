using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WVUPSM.Models.Entities
{
    /// <summary>
    ///     Notifacation table class
    /// </summary>
    [Table("Notifications", Schema = "SM")]
    public class Notification
    {
        /// <summary>
        /// autoincrementing ID
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The User's id that is getting this notification
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Navigation property to the User who is getting this notification
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        /// the user that interacted with 'something' to cause this notification
        /// Notifications in the future could be triggered without another user
        /// </summary>
        public string InteractingUserId { get; set; }

        /// <summary>
        /// Navigation Property for interacting User
        /// </summary>
        [ForeignKey(nameof(InteractingUserId))]
        public User InteractingUser { get; set; }

        /// <summary>
        ///     the comment the interacting user made
        /// </summary>
        public int? CommentId { get; set; }

        /// <summary>
        /// Navigation Property for the Comment the interacting user made
        /// </summary>
        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; }

        /// <summary>
        ///     Creation date of this Notifcation, database seeds this automatically
        /// </summary>
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///     Value of the type of notifications this is
        /// </summary>
        [Required]
        public NotificationType Type { get; set; }

        /// <summary>
        ///     True if this notification has been read, false otherwise
        ///     Past tense by the way
        /// </summary>
        public bool Read { get; set; }
    }

    /// <summary>
    ///     Types of Notifications - Used for building out links
    ///     
    ///     DO NOT CHANGE NUMBERS
    ///     continue incrementing numbers if new notifcation types are added,
    ///     for legacy data we cannot change the number
    /// </summary>
    public enum NotificationType
    {
        Message = 0,
        Comment = 1,
        Follow = 2
    }
}
