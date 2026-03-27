using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Models.Options
{
    public class ParagraphConfig
    {
        [JsonPropertyName("placeholder")]
        public string? placeholder { get; set; }
        [JsonPropertyName("preserveBlank")]
        public bool PreserveBlank { get; set; } = false;
    }
}
