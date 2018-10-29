using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WVUPSM.Models.Entities
{
    /// <summary>
    ///     Represents a Message within the database.
    /// </summary>
    [Table("Messages", Schema = "SM")]
    public class Message
    {
        /// <summary>
        ///     Primary key for a Message
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Sender of this Message
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        ///     Navigation property to the User creating Message
        /// </summary>
        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; }

        /// <summary>
        /// Recipient of this Message
        /// </summary>
        public string ReceiverId { get; set; }

        /// <summary>
        ///     Navigation property to the Recipient of this Message
        /// </summary>
        [ForeignKey(nameof(ReceiverId))]
        public User Recipient { get; set; }

        /// <summary>
        ///     Text content for a Message, max: 300, min: 1
        /// </summary>
        [MaxLength(300)]
        [MinLength(1)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Text { get; set; }

        /// <summary>
        ///     Creation date of this Message, database seeds this automatically
        /// </summary>
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateCreated { get; set; }
    }
}
