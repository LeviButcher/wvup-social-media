using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using WVUPSM.Models.Entities;
using WVUPSM.Service.Tests.APITest.Base;
using Xunit;

namespace WVUPSM.Service.Tests.APITest
{
    [Collection("{Service Testing}")]
    public class FollowControllerTest : BaseTestClass
    {
        public FollowControllerTest()
        {
            RootAddress = "api/Follow";
        }

        [Fact]
        public async void CreateFollow()
        {
            using (var client = new HttpClient())
            {
                string userId = await CreateUserAndReturnId("newUser7");
                string userId2 = await CreateUserAndReturnId("newUser8");

                Follow follow = new Follow()
                {
                    FollowId = userId,
                    UserId = userId2
                };
                var followContent = JsonConvert.SerializeObject(follow);
                var buffer = System.Text.Encoding.UTF8.GetBytes(followContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{ServiceAddress}{RootAddress}/Create/", byteContent);
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void GetFollowers()
        {
            using (var client = new HttpClient())
            {
                string userId = await CreateUserAndReturnId("newUser9");
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Followers/{userId}");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void GetFollowing()
        {
            using (var client = new HttpClient())
            {
                string userId = await CreateUserAndReturnId("newUser10");
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Followers/{userId}");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void DeleteFollow()
        {
            using (var client = new HttpClient())
            {
                
                string userId = await CreateUserAndReturnId("newUser11");
                string userId2 = await CreateUserAndReturnId("newUser12");

                Follow follow = new Follow()
                {
                    FollowId = userId,
                    UserId = userId2
                };
                var followContent = JsonConvert.SerializeObject(follow);
                var buffer = System.Text.Encoding.UTF8.GetBytes(followContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                await client.PostAsync($"{ServiceAddress}{RootAddress}/Create/", byteContent);

                var response = await client.DeleteAsync($"{ServiceAddress}{RootAddress}/Delete/{userId2}/{userId}");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void FollowerCount()
        {
            using (var client = new HttpClient())
            {
                string userId = await CreateUserAndReturnId("newUser13");
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/FollowerCount/{userId}");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void FollowingCount()
        {
            using (var client = new HttpClient())
            {
                string userId = await CreateUserAndReturnId("newUser14");
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/FollowingCount/{userId}");
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}
