using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.Configuration;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.WebServiceAccess
{
    public class WebApiCalls : WebApiCallsBase, IWebApiCalls
    {
        public WebApiCalls(IWebServiceLocator settings) : base(settings)
        {

        }


        //User
        public Task<string> ChangePasswordAsync(string userId, UserProfile user, string currPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreateUserAsync(string password, User user)
        {
            var json = JsonConvert.SerializeObject(user);
            return await SubmitPostRequestAsync(UserCreateUri + password, json);
        }

        public async Task DeleteUserAsync(string userId)
        {
            await SubmitDeleteRequestAsync(UserDeleteUri + userId);
            return;
        }

        public async Task<UserProfile> GetUserAsync(string userId)
        {
            return await GetItemAsync<UserProfile>($"{UserGetUri}{userId}");
        }

        public async Task<IList<UserProfile>> SearchUserAsync(string term)
        {
            return await GetItemListAsync<UserProfile>($"{UserFindUri}{term}");
        }

        public async Task<string> UpdateUserAsync(string userId, UserProfile user)
        {
            var json = JsonConvert.SerializeObject(user);
            return await SubmitPutRequestAsync($"{UserUpdateUri}{userId}", json);
        }

        // Follow
        public async Task<string> CreateFollowAsync(Follow follow)
        {
            var json = JsonConvert.SerializeObject(follow);
            return await SubmitPostRequestAsync(FollowCreateUri, json);
        }

        public async Task DeleteFollowAsync(string userId, string followId)
        {
            await SubmitDeleteRequestAsync(FollowDeleteUri + userId + "/" + followId);
            return;
        }

        public async Task<IList<UserProfile>> GetFollowersAsync(string userid, int skip = 0, int take = 10)
        {
            return await GetItemListAsync<UserProfile>($"{FollowFollowersUri}{userid}?skip={skip}&take={take}");
        }

        public async Task<IList<UserProfile>> GetFollowingAsync(string userid, int skip, int take)
        {
            return await GetItemListAsync<UserProfile>($"{FollowFolloweringUri}{userid}?skip={skip}&take={take}");
        }

        public async Task<bool> IsFollowingAsync(string userId, string followId)
        {
            return (bool)await GetItemAsync<Object>($"{FollowIsFollowingUri}{userId}/{followId}");
        }

        //Post
        public async Task<string> CreatePostAsync(Post post)
        {
            var json = JsonConvert.SerializeObject(post);
            return await SubmitPostRequestAsync(PostCreateUri, json);
        }   
        
        public async Task DeletePostAsync(int postId)
        {
            await SubmitDeleteRequestAsync(PostDeleteUri + postId);
            return;
        }

        public async Task<IList<UserPost>> GetFollowingPostAsync(string userId, int skip, int take)
        {
            return await GetItemListAsync<UserPost>($"{PostFollowingUri}{userId}?skip={skip}&take={take}");
        }

        public async Task<IList<UserPost>> GetMyPostAsync(string userId, int skip = 0, int take = 10)
        {
            return await GetItemListAsync<UserPost>($"{PostMeUri}{userId}?skip={skip}&take={take}");
        }

        public async Task<UserPost> GetPostAsync(int postId)
        {
            return await GetItemAsync<UserPost>($"{PostGetUri}{postId}");
        }

        public async Task<string> UpdatePostAsync(int postId, Post post)
        {
            var json = JsonConvert.SerializeObject(post);
            return await SubmitPutRequestAsync($"{PostUpdateUri}{postId}", json);
        }

        public async Task<IList<UserPost>> GetGroupPostsAsync(int groupId, int skip = 0, int take = 10)
        {
            return await GetItemListAsync<UserPost>($"{PostGroupUri}{groupId}?skip={skip}&take={take}");
        }

        //Groups
        public async Task<string> CreateGroupAsync(Group group)
        {
            var json = JsonConvert.SerializeObject(group);
            return await SubmitPostRequestAsync(GroupCreateUri, json);
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            await SubmitDeleteRequestAsync(GroupDeleteUri + groupId);
            return;
        }

        public async Task<string> UpdateGroupAsync(int groupId, Group group)
        {
            var json = JsonConvert.SerializeObject(group);
            return await SubmitPutRequestAsync($"{GroupUpdateUri}{groupId}", json);
        }

        public async Task<GroupViewModel> GetGroupAsync(int groupId)
        {
            return await GetItemAsync<GroupViewModel>($"{GroupGetUri}{groupId}");
        }

        public async Task<IList<UserProfile>> GetGroupMembersAsync(int groupId)
        {
            return await GetItemListAsync<UserProfile>($"{GroupMembersUri}{groupId}");
        }

        public async Task<UserProfile> GetGroupOwner(int groupId)
        {
            return await GetItemAsync<UserProfile>($"{GroupGetOwnerUri}{groupId}");
        }

        public async Task<IEnumerable<GroupViewModel>> GetUsersGroupsAsync(string userId)
        {
            return await GetItemListAsync<GroupViewModel>($"{GroupUsersUri}{userId}");
        }

        public async Task<List<SelectListItem>> GetGroupsForDropdown(string userId)
        {
            var groups = await GetUsersGroupsAsync(userId);

            var ls = new List<SelectListItem>();

            foreach (GroupViewModel e in groups)
            {
                ls.Add(new SelectListItem
                {
                    Value = e.GroupId.ToString(),
                    Text = e.GroupName,
                });
            }
            return ls;
        }

        //SignInManager                         
        public async Task<string> SignInAsync(LoginViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            return await SubmitPostRequestAsync($"{SignInUri}", json);
        }

        public async Task<string> SignOutAsync()
        {
            return await SubmitPostRequestAsync($"{SignOutUri}", null);
        }
    }
}
