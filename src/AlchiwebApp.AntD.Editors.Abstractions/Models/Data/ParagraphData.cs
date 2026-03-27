using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Paragraph block data model.
    /// </summary>
    public class ParagraphData : IBlockData
    {
        /// <summary>
        /// Text content of the paragraph.
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
