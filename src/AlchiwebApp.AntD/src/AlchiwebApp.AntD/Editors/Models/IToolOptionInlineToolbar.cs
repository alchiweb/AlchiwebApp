using AlchiwebApp.AntD.Editors.Models.Options;
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Models
{
    /// <summary>
    /// Interface for tools that support inline toolbar options.
    /// </summary>
    public interface IToolOptionInlineToolbar
    {
        /// <summary>
        /// Whether the inline toolbar is enabled for the tool.
        /// </summary>
        [JsonPropertyName("inlineToolbar")]
        public bool InlineToolbar { get; set; }
    }
}
