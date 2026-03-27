using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    ///  Checklist block data model.
    /// </summary>
    public class ChecklistData : IBlockData
    {
        /// <summary>
        /// Items in the checklist.
        /// </summary>
        [JsonPropertyName("items")]
        public List<ChecklistItemData>? Items { get; set; }
    }
}
