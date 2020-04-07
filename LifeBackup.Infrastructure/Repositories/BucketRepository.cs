using Amazon.S3;
using Amazon.S3.Model;
using LifeBackup.Core.Buckets;
using LifeBackup.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeBackup.Infrastructure.Repositories
{
    public class BucketRepository : IBucketRepository
    {
        private readonly IAmazonS3 _s3Client;

        public BucketRepository(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<bool> DoesS3BucketExists(string bucketName)
        {
            return await _s3Client.DoesS3BucketExistAsync(bucketName);
        }

        public async Task<CreateS3BucketResponse> CreateBucket(string bucketName)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };
            var response = await _s3Client.PutBucketAsync(putBucketRequest);
            return new CreateS3BucketResponse
            {
                RequestId = response.ResponseMetadata.RequestId,
                BucketName = bucketName
            };
        }

        public async Task<IEnumerable<ListS3BucketResponse>> ListS3Buckets()
        {
            var response = await _s3Client.ListBucketsAsync();
            return response.Buckets.Select(b => new ListS3BucketResponse
            {
                BucketName = b.BucketName,
                CreationDate = b.CreationDate
            });
        }

        public async Task DeleteS3Bucket(string bucketName)
        {
            await _s3Client.DeleteBucketAsync(bucketName);
        }
    }
}
