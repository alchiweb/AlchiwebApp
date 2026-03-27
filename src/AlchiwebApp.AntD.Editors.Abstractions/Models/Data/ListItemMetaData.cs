
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// List item metadata.
    /// </summary>
    public class ListItemMetaData
    {
        /// <summary>
        /// Indicates whether the list item is checked (for task lists).
        /// </summary>
        [JsonPropertyName("checked")]
        public bool? Checked { get; set; }
    }
}
