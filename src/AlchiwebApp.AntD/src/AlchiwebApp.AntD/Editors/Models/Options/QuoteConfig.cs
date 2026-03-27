using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Models.Options
{
    public class QuoteConfig
    {
        [JsonPropertyName("quotePlaceholder")]
        public string? QuotePlaceholder { get; set; }
        [JsonPropertyName("captionPlaceholder")]
        public string? CaptionPlaceholder { get; set; }

    }
}
