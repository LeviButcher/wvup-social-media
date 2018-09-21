﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
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
    }
}
