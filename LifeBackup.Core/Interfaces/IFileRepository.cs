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
    }
}
