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
    [Table("Files", Schema = "SM")]
    public class File
    {
        /// <summary>
        ///    Primary key for a File
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///    Name of a File
        /// </summary>
        [MaxLength(255)]
        public string FileName { get; set; }

        /// <summary>
        ///   Type of Content in File
        /// </summary>
        [MaxLength(100)]
        public string ContentType { get; set; }

        /// <summary>
        ///    File Content 
        /// </summary>
        public byte[] Content { get; set; }
    }
}
