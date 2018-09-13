using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using WVUPSM.Models.Entities;

namespace WVUPSM.Service.Tests.APITest.Base
{
    public abstract class BaseTestClass : IDisposable
    {
        protected string ServiceAddress = "http://localhost:51117/";
        protected string RootAddress = String.Empty;
        SMContext context = new SMContext();

        public void Dispose()
        {

        }

        public async Task<string> CreateUserAndReturnId(string userName)
        {
            
            User user = new User()
            {
                Email = userName + "@wvup.edu",
                UserName = userName
            };
            using (var client = new HttpClient())
            {

                var userContent = JsonConvert.SerializeObject(user);
                var buffer = System.Text.Encoding.UTF8.GetBytes(userContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{ServiceAddress}api/User/Create/Develop@90", byteContent);
            }
            return user.Id;
        }
    }
}
