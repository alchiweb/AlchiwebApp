
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{

    /// <summary>
    /// Checklist item data model.
    /// </summary>
    public class ChecklistItemData
    {
        /// <summary>
        /// Text of the checklist item.
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        /// <summary>
        /// Checked status of the checklist item.
        /// </summary>
        [JsonPropertyName("checked")]
        public bool? Checked { get; set; }
        /// <summary>
        /// Nested items in the checklist item (userful for Checklist???).
        /// </summary>
        [JsonPropertyName("items")]
        public List<ChecklistItemData>? Items { get; set; }
    }
}
