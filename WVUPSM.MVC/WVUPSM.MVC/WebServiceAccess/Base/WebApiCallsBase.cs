using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WVUPSM.MVC.Configuration;

namespace WVUPSM.MVC.WebServiceAccess.Base
{
    public abstract class WebApiCallsBase
    {
        protected readonly string ServiceAddress;
        protected readonly string BaseUri;
        protected readonly string UserBaseUri;
        protected readonly string PostBaseUri;
        protected readonly string FollowBaseUri;
        protected readonly string GroupBaseUri;
        protected readonly string CommentBaseUri;
        protected readonly string MessageBaseUri;
        protected readonly string FileBaseUri;
        protected readonly string NotificationBaseUri;
        protected readonly string TagBaseUri;


        //User
        protected readonly string SignInUri;
        protected readonly string SignOutUri;
        protected readonly string UserGetUri;
        protected readonly string UserDeleteUri;
        protected readonly string UserUpdateUri;
        protected readonly string UserCreateUri;
        protected readonly string UserChangePasswordUri;
        protected readonly string UserFindUri;

        //Post
        protected readonly string PostGetUri;
        protected readonly string PostFollowingUri;
        protected readonly string PostMeUri;
        protected readonly string PostCreateUri;
        protected readonly string PostDeleteUri;
        protected readonly string PostUpdateUri;
        protected readonly string PostGroupUri;

        //Follow
        protected readonly string FollowFollowersUri;
        protected readonly string FollowFolloweringUri;
        protected readonly string FollowCreateUri;
        protected readonly string FollowDeleteUri;
        protected readonly string FollowIsFollowingUri;

        //Group
        protected readonly string GroupCreateUri;
        protected readonly string GroupDeleteUri;
        protected readonly string GroupUpdateUri;
        protected readonly string GroupGetUri;
        protected readonly string GroupSearchUri;
        protected readonly string GroupMembersUri;
        protected readonly string GroupUsersUri;
        protected readonly string GroupMemberCountUri;
        protected readonly string GroupGetOwnerUri;
        protected readonly string GroupIsOwnerUri;
        protected readonly string GroupFindUri;
        protected readonly string GroupIsMemberUri;
        protected readonly string GroupJoinUri;
        protected readonly string GroupLeaveUri;

        //Comment
        protected readonly string CommentCreateUri;
        protected readonly string CommentPostUri;

        //Message
        protected readonly string MessageCreateUri;
        protected readonly string MessageInboxUri;
        protected readonly string MessageConversationUri;
        protected readonly string MessageInboxDetailsUri;

        //File
        protected readonly string FileGetUri;

        //Notifications
        protected readonly string NotificationTodayUri;
        protected readonly string NotificationUnreadUri;
        protected readonly string NotificationReadUri;
        protected readonly string NotificationUnreadCountUri;
        protected readonly string NotificationMarkUri;
        protected readonly string NotificationUnreadPageUri;
        protected readonly string NotificationReadPageUri;

        //Tags
        protected readonly string SearchTagUri;
        protected readonly string SearchInterestUri;

        protected WebApiCallsBase(IWebServiceLocator settings)
        {
            //Setup
            ServiceAddress = settings.ServiceAddress;
            BaseUri = $"{ServiceAddress}api/";
            
            //Base
            UserBaseUri = $"{BaseUri}User/";
            PostBaseUri = $"{BaseUri}Post/";
            FollowBaseUri = $"{BaseUri}Follow/";
            GroupBaseUri = $"{BaseUri}Group/";
            CommentBaseUri = $"{BaseUri}Comment/";
            MessageBaseUri = $"{BaseUri}Message/";
            FileBaseUri = $"{BaseUri}File/";
            NotificationBaseUri = $"{BaseUri}Notification/";
            TagBaseUri = $"{BaseUri}Tag/";

            //User
            SignInUri = $"{UserBaseUri}SignIn/";
            SignOutUri = $"{UserBaseUri}SignOut/";
            UserGetUri = $"{UserBaseUri}Get/";
            UserDeleteUri = $"{UserBaseUri}Delete/";
            UserUpdateUri = $"{UserBaseUri}Update/";
            UserCreateUri = $"{UserBaseUri}Create/";
            UserChangePasswordUri = $"{UserBaseUri}ChangePassword/";
            UserFindUri = $"{UserBaseUri}Find/";

            //Post
            PostGetUri = $"{PostBaseUri}Get/";
            PostFollowingUri = $"{PostBaseUri}Following/";
            PostMeUri = $"{PostBaseUri}Me/";
            PostCreateUri = $"{PostBaseUri}Create/";
            PostDeleteUri = $"{PostBaseUri}Delete/";
            PostUpdateUri = $"{PostBaseUri}Update/";
            PostGroupUri = $"{PostBaseUri}GroupPosts/";

            //Follow
            FollowFollowersUri = $"{FollowBaseUri}Followers/";
            FollowFolloweringUri = $"{FollowBaseUri}Following/";
            FollowCreateUri = $"{FollowBaseUri}Create/";
            FollowDeleteUri = $"{FollowBaseUri}Delete/";
            FollowIsFollowingUri = $"{FollowBaseUri}IsFollowing/";

            //Group
            GroupCreateUri = $"{GroupBaseUri}Create/";
            GroupDeleteUri = $"{GroupBaseUri}Delete/";
            GroupUpdateUri = $"{GroupBaseUri}Update/";
            GroupGetUri = $"{GroupBaseUri}GetGroup/";
            GroupMembersUri = $"{GroupBaseUri}Members/";
            GroupUsersUri = $"{GroupBaseUri}UsersGroups/";
            GroupMemberCountUri = $"{GroupBaseUri}MemberCount/";
            GroupGetOwnerUri = $"{GroupBaseUri}GetOwner/";
            GroupIsOwnerUri = $"{GroupBaseUri}IsOwner/";
            GroupFindUri = $"{GroupBaseUri}Find/";
            GroupIsMemberUri = $"{GroupBaseUri}IsMember/";
            GroupJoinUri = $"{GroupBaseUri}Join/";
            GroupLeaveUri = $"{GroupBaseUri}Leave/";
            GroupSearchUri = $"{GroupBaseUri}Search/";

            //Comment
            CommentCreateUri = $"{CommentBaseUri}Create/";
            CommentPostUri = $"{CommentBaseUri}Post/";

            //Message
            MessageCreateUri = $"{MessageBaseUri}Create/";
            MessageInboxUri = $"{MessageBaseUri}Inbox/";
            MessageConversationUri = $"{MessageBaseUri}Conversation/";
            MessageInboxDetailsUri = $"{MessageBaseUri}InboxDetails/";

            //File
            FileGetUri = $"{FileBaseUri}Get/";

            //Notification
            NotificationTodayUri = $"{NotificationBaseUri}Today/";
            NotificationUnreadUri = $"{NotificationBaseUri}Unread/"; 
            NotificationReadUri = $"{NotificationBaseUri}Read/";
            NotificationUnreadCountUri = $"{NotificationBaseUri}Unread/Count/";
            NotificationMarkUri = $"{NotificationBaseUri}Mark/";
            NotificationUnreadPageUri = $"{NotificationBaseUri}Unread/Page/";
            NotificationReadPageUri = $"{NotificationBaseUri}Read/Page/";

            //Tag
            SearchTagUri = $"{TagBaseUri}Find/";
            SearchInterestUri = $"{TagBaseUri}GetUserInterest/";
        }

        internal async Task<string> GetJsonFromGetResponseAsync(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(uri);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"The Call to {uri} failed. Status code: {response.StatusCode}");
                    }
                    Console.WriteLine("response Successful");
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                //Do something intelligent here if you can
                Console.WriteLine(e);
                throw;
            }
        }

        internal async Task<T> GetItemAsync<T>(string uri) where T : class, new()
        {
            try
            {
                var json = await GetJsonFromGetResponseAsync(uri);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                // intelligent
                Console.WriteLine(e);
                throw;
            }
        }

        internal async Task<IList<T>> GetItemListAsync<T>(string uri) where T : class, new()
        {
            try
            {
                return JsonConvert.DeserializeObject<IList<T>>(await GetJsonFromGetResponseAsync(uri));
            }
            catch (Exception e)
            {
                //intelligent
                Console.WriteLine(e);
                throw;
            }
        }

        protected static async Task<string> ExecuteRequestAndProcessResponse(string uri, Task<HttpResponseMessage> task)
        {
            try
            {
                var response = await task;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"The Call to {uri} failed. Status code: {response.StatusCode}");
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                //something
                Console.WriteLine(e);
                throw;
            }
        }

        protected StringContent CreateStringContent(string json)
        {
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        protected async Task<string> SubmitPostRequestAsync(string uri, string json)
        {
            using (var client = new HttpClient())
            {
                var task = client.PostAsync(uri, CreateStringContent(json));
                return await ExecuteRequestAndProcessResponse(uri, task);
            }
        }

        protected async Task<string> SubmitPutRequestAsync(string uri, string json)
        {
            using (var client = new HttpClient())
            {
                Task<HttpResponseMessage> task = client.PutAsync(uri, CreateStringContent(json));
                return await ExecuteRequestAndProcessResponse(uri, task);
            }
        }

        protected async Task SubmitDeleteRequestAsync(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Task<HttpResponseMessage> deleteAsync = client.DeleteAsync(uri);
                    var response = await deleteAsync;

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(response.StatusCode.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
