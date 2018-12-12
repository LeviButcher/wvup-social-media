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
        Task<IList<UserProfile>> GetUserAsync(int skip = 0, int take = 10);

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
        Task<string> JoinGroupAsync(string userId, int groupId);
        Task<string> LeaveGroupAsync(string userId, int groupId);
        Task<string> UpdateGroupAsync(int groupId, Group group);
        Task<GroupViewModel> GetGroupAsync(int groupId);
        Task<IList<UserProfile>> GetGroupMembersAsync(int groupId, int skip = 0, int take = 20);
        Task<UserProfile> GetGroupOwner(int groupId);
        Task<IList<GroupViewModel>> SearchGroupAsync(string term);
        Task<IEnumerable<GroupViewModel>> GetUsersGroupsAsync(string userId, int skip = 0, int take = 10);
        Task<bool> IsMember(string userId, int groupId);
        Task<List<SelectListItem>> GetGroupsForDropdown(string userId);

        //Comment
        Task<string> CreateCommentAsync(Comment comment);
        Task<IList<CommentViewModel>> GetCommentsAsync(int postId, int skip = 0, int take = 10);

        //Messages
        Task<string> CreateMessageAsync(Message message);
        Task<IList<InboxMessageViewModel>> GetInboxAsync(string userId, int skip = 0, int take = 10);
        Task<IList<MessageViewModel>> GetConversationAsync(string userId, string otherUserId, int skip = 0, int take = 10);
        Task<PagingViewModel> GetInboxDetails(string userId, int pageSize = 10, int pageIndex = 1);

        //File
        Task<WVUPSM.Models.Entities.File> GetFile(int id);

        //Notifications
        Task<IList<NotificationViewModel>> GetTodaysNotifications(string userId);
        Task<IList<NotificationViewModel>> GetUnreadNotifications(string userId, int skip = 0, int take = 10);
        Task<IList<NotificationViewModel>> GetReadNotifications(string userId, int skip = 0, int take = 10);
        Task<Int64> MarkAsRead(int id);
        Task<Int64> GetUnreadCount(string userId);
        Task<PagingViewModel> GetUnreadPageDetails(string userId, int pageSize = 10, int pageIndex = 1);
        Task<PagingViewModel> GetReadPageDetails(string userId, int pageSize = 10, int pageIndex = 1);

        //Tags
        Task<IList<Tag>> SearchTags(string term);
        Task<IList<UserTag>> SearchUserByInterest(string term);
    }
}
