using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.ViewModels;
using WVUPSM.Models.Entities;
using System.Threading.Tasks;

namespace WVUPSM.DAL.Repos.Interfaces
{
    public interface IUserRepo
    {
        IEnumerable<UserProfile> GetAllUsers();
        IEnumerable<UserProfile> GetUsers(int skip = 0, int take = 10);
        IEnumerable<UserProfile> FindUsers(string term);
        Task<User> GetBase(string id);
        UserProfile GetUser(string id);
        //Task<bool> DeleteUserAsync(User user);
        //Task<bool> UpdateUserAsync(User user);
        //Task<UserProfile> CreateUserAsync(User user, String password);
        //Task<bool> ChangePasswordAsync(User user, string currPass, string newPass);
        UserProfileWithUserPosts GetUserPosts();
    }
}
