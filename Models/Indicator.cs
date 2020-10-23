using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace dpaLinkTool.Models
{
    public class Indicator
    {
        [JsonPropertyName("id")]
        public long ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("moduleName")]
        public string ModuleName { get; set; }

        [JsonPropertyName("device")]
        public string Device { get; set; }

        [JsonPropertyName("deviceClass")]
        public string DeviceClass { get; set; }
    }
}
