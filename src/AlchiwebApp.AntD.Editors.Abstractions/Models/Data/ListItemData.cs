
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// List item data model.
    /// </summary>
    public class ListItemData
    {
        /// <summary>
        /// Content of the list item.
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }
        /// <summary>
        /// Metadata associated with the list item.
        /// </summary>
        [JsonPropertyName("meta")]
        public ListItemMetaData? Meta { get; set; }
        /// <summary>
        /// Inner items of the list item (useful for nested lists).
        /// </summary>
        [JsonPropertyName("items")]
        public List<ListItemData>? Items { get; set; }
    }
}
