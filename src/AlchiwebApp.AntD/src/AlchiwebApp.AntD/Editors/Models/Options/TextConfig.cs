using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Models.Options
{
    public class TextConfig
    {
        [JsonPropertyName("placeholder")]
        public string? placeholder { get; set; }
        [JsonPropertyName("preserveBlank")]
        public bool PreserveBlank { get; set; } = false;
        [JsonPropertyName("allowEnterKeyDown")]
        public bool? AllowEnterKeyDown { get; set; } = false;
        [JsonPropertyName("hidePopoverItem")]
        public bool? HidePopoverItem { get; set; } = false;
        [JsonPropertyName("hideToolbar")]
        public bool? HideToolbar { get; set; } = false;
        [JsonPropertyName("wrapElement")]
        public string? WrapElement { get; set; }
    }
}
