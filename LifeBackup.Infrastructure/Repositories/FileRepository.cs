using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using LifeBackup.Core.Files;
using LifeBackup.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LifeBackup.Infrastructure.Repositories
{
    public class FileRepository: IFileRepository
    {
        private readonly IAmazonS3 _s3Client;

        public FileRepository(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> files)
        {
            var response = new List<string>();
            foreach (var file in files)
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = file.FileName,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                using(var fileTransferUtility = new TransferUtility(_s3Client))
                {
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }

                var expiryUrlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    Expires = DateTime.UtcNow.AddDays(1)
                };

                var url = _s3Client.GetPreSignedURL(expiryUrlRequest);
                response.Add(url);
             
            }
            return new AddFileResponse
            {
                PreSignedUrls = response
            };

        }
    }
}
