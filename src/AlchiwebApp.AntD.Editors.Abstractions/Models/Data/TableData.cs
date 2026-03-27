using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Table block data model.
    /// </summary>
    public class TableData : IBlockData
    {
        /// <summary>
        /// Gets or sets the content for table blocks.
        /// </summary>
        [JsonPropertyName("content")]
        public List<List<string?>>? Content { get; set; }
        /// <summary>
        /// Gets or sets the flag indicating whether table blocks have headings.
        /// </summary>
        [JsonPropertyName("withHeadings")]
        public bool? WithHeadings { get; set; }
    }
}
