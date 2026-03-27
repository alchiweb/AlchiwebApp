using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Models
{
    /// <summary>
    /// EditorJs configuration model.
    /// </summary>
    public class EditorJsConfiguration
    {
        /// <summary>
        /// Default block type.
        /// </summary>
        [JsonPropertyName("defaultBlock")]
        public string DefaultBlock { get; set; } = "paragraph";
        /// <summary>
        /// Tools configurations.
        /// </summary>
        [JsonPropertyName("tools")]
        public ToolsConfigurations? Tools  { get; set; }
        /// <summary>
        /// Paceholder text for the editor.
        /// </summary>
        [JsonPropertyName("placeholder")]
        public string? Placeholder { get; set; }
        /// <summary>
        /// Autofocus the editor on load.
        /// </summary>
        [JsonPropertyName("autofocus")]
        public bool? Autofocus { get; set; }
    }
}
