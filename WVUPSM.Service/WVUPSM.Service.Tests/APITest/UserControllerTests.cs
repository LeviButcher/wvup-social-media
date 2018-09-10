using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
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
                    Email = "lbutche3@wvup.edu",
                    UserName = "lbutche"
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
                //User user = new User()
                //{
                //    Email = "lbutche3@wvup.edu",
                //    UserName = "lbutche"
                //};
                //var userContent = JsonConvert.SerializeObject(user);
                //var buffer = System.Text.Encoding.UTF8.GetBytes(userContent);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Get/718726ec-ba51-4927-9e80-baf65d75fba3");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void GetUsers()
        {
            using (var client = new HttpClient())
            {
                //User user = new User()
                //{
                //    Email = "lbutche3@wvup.edu",
                //    UserName = "lbutche"
                //};
                //var userContent = JsonConvert.SerializeObject(user);
                //var buffer = System.Text.Encoding.UTF8.GetBytes(userContent);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Get/?skip=0&take=3");
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void DeleteUser()
        {
            using (var client = new HttpClient())
            {
                //User user = new User()
                //{
                //    Email = "lbutche3@wvup.edu",
                //    UserName = "lbutche"
                //};
                //var userContent = JsonConvert.SerializeObject(user);
                //var buffer = System.Text.Encoding.UTF8.GetBytes(userContent);
                //var byteContent = new ByteArrayContent(buffer);
                //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.DeleteAsync($"{ServiceAddress}{RootAddress}/Delete/6d2de6c2-b73a-4720-a92e-4fee0df3d25d");
                Assert.True(response.IsSuccessStatusCode);
            }
        }





    }
}
