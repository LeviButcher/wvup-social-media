using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Service.Tests.APITest.Base;
using Xunit;

namespace WVUPSM.Service.Tests.APITest
{
    [Collection("{Service Testing}")]
    public class UserControllerTests : BaseTestClass
    {


        public UserControllerTests()
        {
            RootAddress = "api/User";
        }



        [Fact]
        public async void CreateUser()
        {
            using(var client = new HttpClient())
            {
                User user = new User()
                {
                    Email = "user@wvup.edu",
                    UserName = "User"
                };
                var userContent = JsonConvert.SerializeObject(user);
                var buffer = System.Text.Encoding.UTF8.GetBytes(userContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{ServiceAddress}{RootAddress}/Create/Develop@90", byteContent);
                Assert.True(response.IsSuccessStatusCode);
            }
        }


        [Fact]
        public async void GetUser()
        {
            using (var client = new HttpClient())
            {
                string userId = await CreateUserAndReturnId("newUser1");
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Get/{userId}");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void GetUsers()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Get/?skip=0&take=3");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void DeleteUser()
        {
            using (var client = new HttpClient())
            {
                string userId = await CreateUserAndReturnId("newUser2");
                var response = await client.DeleteAsync($"{ServiceAddress}{RootAddress}/Delete/{userId}");
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}
