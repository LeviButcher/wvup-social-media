using System.ComponentModel.DataAnnotations.Schema;

namespace WVUPSM.Models.Entities
{
    /// <summary>
    ///     Represents the Follow table within the database
    /// </summary>
    [Table("Follows", Schema = "SM")]
    public class Follow
    {
        /// <summary>
        /// The User's id that is starting to follow someone
        /// </summary>
        public string UserId { get; set; }        

        /// <summary>
        ///     Navigation property to the User starting to follow someone
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        ///    The User's id who is the person that is going to be followed 
        /// </summary>
        public string FollowId { get; set; }

        /// <summary>
        ///     Navigation property to the person being followed
        /// </summary>
        [ForeignKey(nameof(FollowId))]
        public User Person { get; set; }
    }
}
