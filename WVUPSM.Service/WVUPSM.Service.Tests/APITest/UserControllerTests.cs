using Microsoft.AspNetCore.Identity;
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
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/Get/36362f75-2544-4f14-893e-3096a52063d0");
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

                var response = await client.DeleteAsync($"{ServiceAddress}{RootAddress}/Delete/36362f75-2544-4f14-893e-3096a52063d0");
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}
