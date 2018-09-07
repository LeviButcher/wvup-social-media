using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.ViewModels;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.Repos.Interfaces
{
    public interface IUserRepo
    {
        IEnumerable<UserProfile> GetAllUsers();
        IEnumerable<UserProfile> GetUsers(int skip = 0, int take = 10);
        IEnumerable<UserProfile> FindUsers(string term);
        UserProfile GetUser(string id);
        int DeleteUser(User user);
        UserProfile UpdateUser(User user);
        UserProfile CreateUser(User user);
        bool ChangePassword(string userId, string currPass, string newPass);
        UserProfileWithUserPosts GetUserPosts();
    }
}
