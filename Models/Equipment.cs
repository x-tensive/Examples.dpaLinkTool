using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace dpaLinkTool.Models
{
    public class Equipment
    {
        [JsonPropertyName("id")]
        public long ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("departmentName")]
        public string DepartmentName { get; set; }
    }
}
