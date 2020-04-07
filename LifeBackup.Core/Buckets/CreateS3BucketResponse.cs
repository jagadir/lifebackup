using System;
using System.Collections.Generic;
using System.Text;

namespace LifeBackup.Core.Buckets
{
    public class CreateS3BucketResponse
    {
        public string RequestId { get; set; }
        public string BucketName { get; set; }
    }
}
