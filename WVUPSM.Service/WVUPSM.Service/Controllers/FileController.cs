using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;

namespace WVUPSM.Service.Controllers
{
    /// <summary>
    ///     Controller for Posts
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class FileController : Controller
    {
        private IFileRepo _fRepo;

        public FileController(IFileRepo fRepo)
        {
            _fRepo = fRepo;
        }

        [HttpGet("{fileId}")]
        public IActionResult Get(int fileId)
        {
            Models.Entities.File file = _fRepo.GetFile(fileId);
            if (file == null)
            {
                return BadRequest();
            }

            return Json(file);
        }

        [HttpPost]
        public int Create(IFormFile file)
        {

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}

            // full path to file in temp location
            int fileId = -1;
                if (file.Length > 0)
                {
                    using (var reader = new BinaryReader(file.OpenReadStream()))
                    {
                        byte[] fileContent = reader.ReadBytes((int)file.Length);

                        Models.Entities.File newFile = new Models.Entities.File();
                        newFile.FileName = file.FileName;
                        newFile.Content = fileContent;
                        newFile.ContentType = file.ContentType;

                        fileId = _fRepo.GetFileByProps(file.FileName, fileContent, file.ContentType);

                        if(fileId == -1)
                        {
                            _fRepo.CreateFile(newFile);
                        }
                        else
                        {
                            return fileId;
                        }

                        fileId = _fRepo.GetFileByProps(file.FileName, fileContent, file.ContentType);
                    }
                }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return fileId;
        }

        [HttpPost]
        public int Delete(Models.Entities.File file)
        {
            return _fRepo.DeleteFile(file);           
        }

    }
}
