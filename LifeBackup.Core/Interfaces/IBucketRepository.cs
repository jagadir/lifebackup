using LifeBackup.Core.Buckets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LifeBackup.Core.Interfaces
{
    public interface IBucketRepository
    {
        Task<bool> DoesS3BucketExists(string bucketName);
        Task<CreateS3BucketResponse> CreateBucket(string bucketName);
        Task<IEnumerable<ListS3BucketResponse>> ListS3Buckets();
        Task DeleteS3Bucket(string bucketName);
    }
}
