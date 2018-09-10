using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using WVUPSM.Models.Entities;
using WVUPSM.Service.Tests.APITest.Base;
using Xunit;



namespace WVUPSM.Service.Tests.APITest
{
    [Collection("{Service Testing}")]
    public class PostControllerTest : BaseTestClass
    {
        public PostControllerTest()
        {
            RootAddress = "api/Post";
        }

        [Fact]
        public async void CreatePost()
        {
            using (var client = new HttpClient())
            {
                Post post = new Post()
                {
                    Text = "This is a test post",
                    UserId = "36362f75-2544-4f14-893e-3096a52063d0"
                };
                var postContent = JsonConvert.SerializeObject(post);
                var buffer = System.Text.Encoding.UTF8.GetBytes(postContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{ServiceAddress}{RootAddress}/Create/", byteContent);
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void GetPost()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Get/3");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void GetAllPost()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Get/");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void GetMyPosts()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Me/052b10eb-c29e-4108-8cec-8937e17f1866");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void UpdatePost()
        {
            using (var client = new HttpClient())
            {
                Post post = new Post()
                {
                    Text = "I like dogs more",
                    UserId = "58fe425b-5f05-4d85-b0d4-ce49c37e3733",
                    Id = 1                    
                };
                var postContent = JsonConvert.SerializeObject(post);
                var buffer = System.Text.Encoding.UTF8.GetBytes(postContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PutAsync($"{ServiceAddress}{RootAddress}/Update/1", byteContent);
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void DeletePost()
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{ServiceAddress}{RootAddress}/Delete/1");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void GetFollowingPosts()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Following/58fe425b-5f05-4d85-b0d4-ce49c37e3733");
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}
