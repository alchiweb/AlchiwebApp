using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Link item data model.
    /// </summary>
    public class ListData : IBlockData
    {
        /// <summary>
        /// Items in the list.
        /// </summary>
        [JsonPropertyName("items")]
        public List<ListItemData>? Items { get; set; }
        /// <summary>
        /// Gets or sets the style for list blocks (e.g., ordered or unordered).
        /// </summary>
        [JsonPropertyName("style")]
        public string? Style { get; set; }
    }
}
