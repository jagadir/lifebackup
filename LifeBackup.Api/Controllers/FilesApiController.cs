using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeBackup.Core.Files;
using LifeBackup.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeBackup.Api.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesApiController : ControllerBase
    {
        private readonly IFileRepository _repository;

        public FilesApiController(IFileRepository repository)
        {
            _repository = repository;
        }


        [HttpPost]
        [Route("{bucketName}/add")]
        public async Task<ActionResult<AddFileResponse>> AddFiles(string bucketName, IList<IFormFile> files)
        {
            if(files == null)
            {
                return BadRequest("the request doesn't contain any files to be deleted.");
            }

            var response = await _repository.UploadFiles(bucketName, files);
            return Ok(response);
        }

    }
}