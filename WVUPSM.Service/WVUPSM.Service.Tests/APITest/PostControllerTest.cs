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
                string userId = await CreateUserAndReturnId("newUser3");
                Post post = new Post()
                {
                    Text = "This is a test post",
                    UserId = userId
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
            string userId = await CreateUserAndReturnId("newUser4");

            using (var client = new HttpClient())
            {
                Post post = new Post()
                {
                    Text = "This is a test post",
                    UserId = userId
                };
                var postContent = JsonConvert.SerializeObject(post);
                var buffer = System.Text.Encoding.UTF8.GetBytes(postContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                await client.PostAsync($"{ServiceAddress}{RootAddress}/Create/", byteContent);

                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Index/{userId}");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void UpdatePost()
        {
            using (var client = new HttpClient())
            {
                //Creates new post
                string userId = await CreateUserAndReturnId("newUser5");
                Post post = new Post()
                {
                    Text = "This is a test post",
                    UserId = userId
                };
                var postContent = JsonConvert.SerializeObject(post);
                var buffer = System.Text.Encoding.UTF8.GetBytes(postContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Post newPost = new Post()
                {
                    Text = "I like dogs more",
                    UserId = userId,
                    Id = 1                    
                };
                var upsdatedPostContent = JsonConvert.SerializeObject(post);
                var newBuffer = System.Text.Encoding.UTF8.GetBytes(postContent);
                var newByteContent = new ByteArrayContent(buffer);
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
                string userId = await CreateUserAndReturnId("newUser6");
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Following/{userId}");
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}
