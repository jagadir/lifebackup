using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LifeBackup.Core.Files
{
    public class AddJsonObjectRequest
    {
        [JsonProperty("id")]

        public Guid Id { get; set; }
        [JsonProperty("timesetn")]
        public DateTime TimeSent { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
