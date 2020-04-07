using LifeBackup.Core.Files;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LifeBackup.Core.Interfaces
{
    public interface IFileRepository
    {
        Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> files);
        Task<IEnumerable<ListFileResponse>> ListFiles(string bucketName);
        Task DownloadFile(string bucketName, string fileName);
        Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName);
        Task AddJsonObject(string bucketName, AddJsonObjectRequest request);
        Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string fileName);
    }
}
