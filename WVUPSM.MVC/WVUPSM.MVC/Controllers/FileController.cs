using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WVUPSM.MVC.Configuration;
using WVUPSM.MVC.WebServiceAccess.Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVUPSM.MVC.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class FileController : Controller
    {
        public IWebApiCalls WebApiCalls { get; }
        public IWebServiceLocator Settings { get; }

        public FileController(IWebApiCalls webApiCalls, IWebServiceLocator settings)
        {
            WebApiCalls = webApiCalls;
            Settings = settings;
        }

        // GET: /<controller>/
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var file = await WebApiCalls.GetFile(id);
            return File(file.Content, file.ContentType, file.FileName);
        }

        
        public async Task<int> Create(IFormFile file)
        {
            var fileId = -1;
            if (file == null) return fileId;
            using (var client = new HttpClient())
            {
                var ServiceAddress = Settings.ServiceAddress;
               
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(file.OpenReadStream())
                        {
                            Headers =
                        {
                            ContentLength = file.Length,
                            ContentType = new MediaTypeHeaderValue(file.ContentType)
                        }
                        }, "file", fileName);
                        var response = await client.PostAsync($"{ServiceAddress}api/File/Create/", content);
                        return int.Parse(await response.Content.ReadAsStringAsync());
                    }
                }
                else
                {
                    return fileId;
                }
            }
        }
    }
}
