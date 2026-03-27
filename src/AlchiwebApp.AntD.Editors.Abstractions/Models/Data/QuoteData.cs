using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Quote alignment options.
    /// </summary>
    public class QuoteData : IBlockData
    {
        /// <summary>
        /// Text content of the quote.
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }
        /// <summary>
        /// Gets or sets the caption for quote, image or embed blocks.
        /// </summary>
        [JsonPropertyName("caption")]
        public string? Caption { get; set; }
        /// <summary>
        /// Gets or sets the alignment for quote blocks.
        /// </summary>
        [JsonPropertyName("alignment")]
        public QuoteAlignmentEnum Alignment { get; set; } = QuoteAlignmentEnum.Left;
    }
}
