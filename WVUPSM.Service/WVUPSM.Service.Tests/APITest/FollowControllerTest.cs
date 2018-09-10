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
                Follow follow = new Follow()
                {
                    FollowId = "36362f75-2544-4f14-893e-3096a52063d0",
                    UserId = "ea3dbce6-90f4-459d-a0f1-91f8109bc84a"
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
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Followers/36362f75-2544-4f14-893e-3096a52063d0");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void GetFollowing()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Followers/052b10eb-c29e-4108-8cec-8937e17f1866");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void DeleteFollow()
        {
            using (var client = new HttpClient())
            {
                //Follow follow = new Follow()
                //{
                //    FollowId = "36362f75-2544-4f14-893e-3096a52063d0",
                //    UserId = "ea3dbce6-90f4-459d-a0f1-91f8109bc84a"
                //};
                //var followContent = JsonConvert.SerializeObject(follow);
                //var buffer = System.Text.Encoding.UTF8.GetBytes(followContent);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.DeleteAsync($"{ServiceAddress}{RootAddress}/Delete/ea3dbce6-90f4-459d-a0f1-91f8109bc84a/36362f75-2544-4f14-893e-3096a52063d0");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void FollowerCount()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/FollowerCount/052b10eb-c29e-4108-8cec-8937e17f1866");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void FollowingCount()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/FollowingCount/052b10eb-c29e-4108-8cec-8937e17f1866");
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}
