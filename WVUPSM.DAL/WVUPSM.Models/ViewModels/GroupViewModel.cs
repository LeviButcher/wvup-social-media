using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    ///     Used for passing a Group to a View
    /// </summary>
    public class GroupViewModel
    {
        /// <summary>
        ///     Id of Group
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        ///     Name of Group
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        ///     Group Bio
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        /// <summary>
        ///     Date Group was created
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///     Number of members in Group
        /// </summary>
        public int MemberCount { get; set; }

        /// <summary>
        ///     Id of User that Created Group
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        ///     UserName of User that Created Group
        /// </summary>
        public string OwnerUserName { get; set; }
    }
}
