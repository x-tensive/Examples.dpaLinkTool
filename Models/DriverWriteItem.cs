using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace dpaLinkTool.Models
{
    public class DriverWriteItem
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("values")]
        public string[] Values { get; set; }
    }
}
