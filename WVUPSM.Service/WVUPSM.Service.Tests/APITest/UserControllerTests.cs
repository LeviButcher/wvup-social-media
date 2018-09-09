using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
    }
}
