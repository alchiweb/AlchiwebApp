using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models
{
    /// <summary>
    /// Data model for EditorJS content.
    /// </summary>
    public class EditorJsData
    {
        /// <summary>
        /// Timestamp of the last modification in ticks.
        /// </summary>
        [JsonPropertyName("time")]
        public long Time { get; set; } = DateTime.UtcNow.Ticks;
        /// <summary>
        /// List of content blocks.
        /// </summary>
        [JsonPropertyName("blocks")]
        public List<EditorJsBlock> Blocks  { get; set; } = [];
        /// <summary>
        /// Version of EditorJS used.
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version { get; set; } = "";
    }
}
