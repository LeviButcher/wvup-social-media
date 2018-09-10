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


        public async void CreateFollow()
        {
            using (var client = new HttpClient())
            {
                Follow follow = new Follow()
                {
                    FollowId = "6d2de6c2-b73a-4720-a92e-4fee0df3d25d",
                    UserId = "9c72b184-7cfb-4f91-8c03-c01b0f6d7376"
                };
                var followContent = JsonConvert.SerializeObject(follow);
                var buffer = System.Text.Encoding.UTF8.GetBytes(followContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{ServiceAddress}{RootAddress}/Create/", byteContent);
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}
