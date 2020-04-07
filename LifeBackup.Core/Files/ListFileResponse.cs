using System;
using System.Collections.Generic;
using System.Text;

namespace LifeBackup.Core.Files
{
   public class ListFileResponse
    {
        public string BucketName { get; set; }
        public string FileName { get; set; }

        public string Owner { get; set; }
        public long FileSize { get; set; }
    }
}
