using AlchiwebApp.AntD.Editors.Models.Options;
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Models
{
    public class ToolOptions : IToolOptionInlineToolbar
    {
        [JsonPropertyName("inlineToolbar")]
        public bool InlineToolbar { get; set; } = false;
        [JsonPropertyName("shortcut")]
        public string? Shortcut { get; set; }
    }
    public class ToolOptions<TConfig> : ToolOptions
    {
        [JsonPropertyName("config")]
        public TConfig? Config { get; set; }
    }
}
