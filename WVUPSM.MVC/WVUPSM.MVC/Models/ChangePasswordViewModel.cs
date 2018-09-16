using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVUPSM.MVC.Models
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; }

        public string CurrentPassword { get; set; }
         
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
