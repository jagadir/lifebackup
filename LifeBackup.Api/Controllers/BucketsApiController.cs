using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeBackup.Core.Buckets;
using LifeBackup.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeBackup.Api.Controllers
{
    [Route("api/bucket")]
    [ApiController]
    public class BucketsApiController : ControllerBase
    {
        private readonly IBucketRepository _repository;

        public BucketsApiController(IBucketRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("test")]
        public ActionResult TestApi()
        {
            return Ok("success");
        }
        [HttpPost]
        [Route("create/{bucketName}")]
        public async Task<ActionResult<CreateS3BucketResponse>> CreateS3Bucket([FromRoute] string bucketName)
        {
            var bucketExist = await _repository.DoesS3BucketExists(bucketName);
            if(bucketExist)
            {
                return BadRequest("S3 bucket already exists");
            }
            var result = await _repository.CreateBucket(bucketName);
            return Ok(result);
        }    

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<ListS3BucketResponse>> ListS3Buckets()
        {
            var result = await _repository.ListS3Buckets();

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{bucketname}")]
        public async Task<ActionResult> DeleteS3Bucket(string bucketName)
        {
            await _repository.DeleteS3Bucket(bucketName);
            return Ok();
        }
     }
}