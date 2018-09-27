using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.MVC.WebServiceAccess.Base
{
    public interface IWebApiCalls
    {
        //User
        Task<string> SignInAsync(LoginViewModel model);
        Task<string> SignOutAsync();
        Task<UserProfile> GetUserAsync(string userId);
        Task<string> CreateUserAsync(string password, User user);
        Task<string> UpdateUserAsync(string userId, UserProfile user);
        Task DeleteUserAsync(string userId);
        Task<IList<UserProfile>> SearchUserAsync(string term);
        Task<string> ChangePasswordAsync(string userId, UserProfile user, string currPassword, string newPassword);

        //Post
        Task<UserPost> GetPostAsync(int postId);
        Task<IList<UserPost>> GetFollowingPostAsync(string userId, int skip = 0, int take = 10);
        Task<IList<UserPost>> GetMyPostAsync(string userId, int skip = 0, int take = 10);
        Task<IList<UserPost>> GetGroupPostsAsync(int groupId, int skip = 0, int take = 10);
        Task<string> CreatePostAsync(Post post);
        Task<string> UpdatePostAsync(int postId, Post post);
        Task DeletePostAsync(int postId);
        Task<bool> IsFollowingAsync(string userId, string followId);



        //Follow
        Task<IList<UserProfile>> GetFollowersAsync(string userid, int skip = 0, int take = 10);
        Task<IList<UserProfile>> GetFollowingAsync(string userid, int skip = 0, int take = 10);
        Task<string> CreateFollowAsync(Follow follow);
        Task DeleteFollowAsync(string userId, string followId);

        //Group
        Task<string> CreateGroupAsync(Group group);
        Task DeleteGroupAsync(int groupId);
        Task<string> UpdateGroupAsync(int groupId, Group group);
        Task<GroupViewModel> GetGroupAsync(int groupId);
        Task<IList<UserProfile>> GetGroupMembersAsync(int groupId);
        Task<UserProfile> GetGroupOwner(int groupId);
        Task<IEnumerable<GroupViewModel>> GetUsersGroupsAsync(string userId);
        Task<List<SelectListItem>> GetGroupsForDropdown(string userId);

    }
}
