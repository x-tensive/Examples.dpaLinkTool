using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace dpaLinkTool.Models
{
    public class IndicatorValue
    {
        [JsonPropertyName("dateTime")]
        public DateTimeOffset TimeStamp { get; set; }

        [JsonPropertyName("value")]
        public JsonElement Value { get; set; }
    }
}
