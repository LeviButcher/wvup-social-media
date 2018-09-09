using WVUPSM.Service.Tests.APITest.Base;
using Xunit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using WVUPSM.Models.Entities;



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
                    UserId = "da0b8732-ddf2-43e4-bd5f-7f6ad9331e22"
                };
                var postContent = JsonConvert.SerializeObject(post);
                var buffer = System.Text.Encoding.UTF8.GetBytes(postContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{ServiceAddress}{RootAddress}/Create/", byteContent);
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}
