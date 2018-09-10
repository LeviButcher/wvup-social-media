﻿using Newtonsoft.Json;
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

        //Follow
        protected readonly string FollowFollowersUri;
        protected readonly string FollowFolloweringUri;
        protected readonly string FollowCreateUri;
        protected readonly string FollowDeleteUri;

        protected WebApiCallsBase(IWebServiceLocator settings)
        {
            //Setup
            ServiceAddress = settings.ServiceAddress;
            BaseUri = $"{ServiceAddress}api/";
            
            //Base
            UserBaseUri = $"{BaseUri}User/";
            PostBaseUri = $"{BaseUri}Post/";
            FollowBaseUri = $"{BaseUri}Follow/";

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

            //Follow
            FollowFollowersUri = $"{FollowBaseUri}Followers/";
            FollowFolloweringUri = $"{FollowBaseUri}Following/";
            FollowCreateUri = $"{FollowBaseUri}Create/";
            FollowDeleteUri = $"{FollowBaseUri}Delete/";
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
