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


        [HttpGet]
        [Route("{bucketName}/list")]
        public async Task<ActionResult<IEnumerable<ListFileResponse>>> ListFiles(string bucketName)
        {
            var response = await _repository.ListFiles(bucketName);
            return Ok(response);
        }


        [HttpGet]
        [Route("{bucketName}/download/{fileName}")]
        public async Task<ActionResult> DownloadFile(string bucketName, string fileName)
        {
            await _repository.DownloadFile(bucketName, fileName);
            return Ok();
        }

        [HttpDelete]
        [Route("{bucketName}/delete/{fileName}")]
        public async Task<ActionResult<DeleteFileResponse>> DeleteFile(string bucketName, string fileName)
        {
            var response = await _repository.DeleteFile(bucketName, fileName);
            return Ok(response);
        }

        [HttpPost]
        [Route("{bucketName}/addjsonobject")]
        public async Task<ActionResult> AddJsonObject(string bucketName, AddJsonObjectRequest request)
        {
            await _repository.AddJsonObject(bucketName, request);
            return Ok();
        }

        [HttpGet]
        [Route("{bucketName}/getjsonobject/{fileName}")]
        public async Task<ActionResult<GetJsonObjectResponse>> GetJsonObject(string bucketName, string fileName)
        {
            var response = await _repository.GetJsonObject(bucketName, fileName);
            return Ok(response);
        }
    }
}