using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.Initiliazers
{
    public static class SampleData
    {
        public static IEnumerable<User> GetUsers()
        {
            List<User> userList = new List<User>();
            User levi = new User()
            {
                Email = "leviB@Develop.com",
                NormalizedEmail = "LEVIB@DEVELOP.COM",
                UserName = "leviB",
                NormalizedUserName = "LEVIB",
            };
            return userList;
        }
    }
}
