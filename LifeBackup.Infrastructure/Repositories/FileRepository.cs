using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using LifeBackup.Core.Files;
using LifeBackup.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task<IEnumerable<ListFileResponse>> ListFiles(string bucketName)
        {
            var response = _s3Client.ListObjectsAsync(bucketName);

            return response.Result.S3Objects.Select(o => new ListFileResponse
            {
                BucketName = o.BucketName,
                FileName = o.Key,
                Owner = o.Owner.DisplayName,
                FileSize = o.Size
            });
        }

        public async Task DownloadFile(string bucketName, string fileName)
        {
            var pathAndFileName = $"C:\\S3Temp\\{fileName}";
            var downloadRequest = new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = fileName,
                FilePath = pathAndFileName
            };

            using(var transferUtility = new TransferUtility(_s3Client))
            {
                await transferUtility.DownloadAsync(downloadRequest);
            }
        }

        public async Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName)
        {
            var multiObjectDeleteRequest = new DeleteObjectsRequest
            {
                BucketName=bucketName
            };

            multiObjectDeleteRequest.AddKey(fileName);
            var response = await _s3Client.DeleteObjectsAsync(multiObjectDeleteRequest);
            return new DeleteFileResponse
            {
                NoOfDeletedObjects = response.DeletedObjects.Count
            };
        }

        public async Task AddJsonObject(string bucketName, AddJsonObjectRequest request)
        {
            var createdOn = DateTime.UtcNow;
            var s3Key = $"{createdOn:yyyy}/{createdOn:MM}/{createdOn:dd}/{request.Id}";
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = s3Key,
                ContentBody = JsonConvert.SerializeObject(request)
            };
            await _s3Client.PutObjectAsync(putObjectRequest);

        }

        public async Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            var response = await _s3Client.GetObjectAsync(request);

            using(var reader = new StreamReader(response.ResponseStream))
            {
                var contents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<GetJsonObjectResponse>(contents);
            }
        }
    }
}

