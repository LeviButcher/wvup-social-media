using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WVUPSM.Models.Entities
{
    /// <summary>
    ///     Represents the User table in the Database. 
    /// </summary>
    /// <remarks>
    ///     The User extends <see cref="IdentityUser"/>
    ///     Object properties our navigation properties used for EF
    /// </remarks>
    [Table("Users", Schema = "SM")]
    public class User : IdentityUser
    {
        /// <summary>
        ///  All the users that this user is following :
        ///  <c>Follow.userId == this.userId</c>.
        /// </summary>
        /// <remarks>
        ///     Will be null if EF includes is not used.
        /// </remarks>
        [InverseProperty(nameof(Follow.User))]
        public List<Follow> Following { get; set; } = new List<Follow>();

        
        /// <summary>
        /// All the users that is following this user :
        /// <c>Follow.followId == this.userId</c>.
        /// </summary>
        [InverseProperty(nameof(Follow.Person))]
        public List<Follow> Followers { get; set; } = new List<Follow>();

        /// <summary>
        ///     All posts created by this user.
        /// </summary>
        [InverseProperty(nameof(Post.User))]
        public List<Post> Posts { get; set; } = new List<Post>();

        /// <summary>
        ///     All Groups where this user is a member.
        /// </summary>
        [InverseProperty(nameof(UserGroup.User))]
        public List<UserGroup> Groups { get; set; } = new List<UserGroup>();

        /// <summary>
        ///     All comments created by this user.
        /// </summary>
        [InverseProperty(nameof(Comment.User))]
        public List<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        ///     Navigation Property for Sent messages
        /// </summary>
        [InverseProperty(nameof(Message.Sender))]
        public List<Message> SentMessages { get; set; } = new List<Message>();

        /// <summary>
        ///     Navigation Property for received messages
        /// </summary>
        [InverseProperty(nameof(Message.Recipient))]
        public List<Message> RecievedMessages { get; set; } = new List<Message>();

        /// <summary>
        ///    Bio set by the user
        /// </summary>
        [MaxLength(400)]
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        /// <summary>
        ///    Bio set by the user
        /// </summary>
        [MaxLength(100)]
        [DataType(DataType.MultilineText)]
        public string Occupation { get; set; }

        /// <summary>
        ///    Bio set by the user
        /// </summary>
        [MaxLength(100)]
        [DataType(DataType.MultilineText)]
        public string Major { get; set; }

        /// <summary>
        ///     Profile Picture -- Foriegn key to File table <see cref="File"/>
        /// </summary>
        public int? FileId { get; set; }

        /// <summary>
        ///     Navigation property to the File table <see cref="File"/>
        /// </summary>
        [ForeignKey(nameof(FileId))]
        public File File { get; set; }

        /// <summary>
        ///     Header Picture -- Foriegn key to File table <see cref="File"/>
        /// </summary>
        public int? HeaderPicId { get; set; }

        /// <summary>
        ///     Navigation property to the File table <see cref="File"/>
        /// </summary>
        [ForeignKey(nameof(HeaderPicId))]
        public File HeaderPic { get; set; }

        /// <summary>
        ///     All Tags created by this user.
        /// </summary>
        [InverseProperty(nameof(UserTag.User))]
        public List<UserTag> UserTags { get; set; } = new List<UserTag>();
    }
}
