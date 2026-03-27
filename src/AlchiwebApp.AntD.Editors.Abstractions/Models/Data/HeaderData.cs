using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Header block data model.
    /// </summary>
    public class HeaderData : IBlockData
    {
        /// <summary>
        /// The text content of the header.
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }
        /// <summary>
        /// Gets or sets the heading level for header blocks.
        /// </summary>
        [JsonPropertyName("level")]
        public int? Level { get; set; }

    }
}
