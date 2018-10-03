using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    public class GroupViewModel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public int MemberCount { get; set; }

        public string OwnerId { get; set; }

        public string OwnerUserName { get; set; }
    }
}
