using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Embed block data model.
    /// </summary>
    public class EmbedData : IBlockData
    {
        /// <summary>
        /// Gets or sets the caption for quote, image or embed blocks.
        /// </summary>
        [JsonPropertyName("caption")]
        public string? Caption { get; set; }
        /// <summary>
        /// Gets or sets the service associated with the embed block.
        /// </summary>
        [JsonPropertyName("service")]
        public EmbedServiceEnum? Service { get; set; }

        /// <summary>
        /// Gets or sets the source URL for the embed block.
        /// </summary>
        [JsonPropertyName("source")]
        public string? Source { get; set; }

        /// <summary>
        /// Gets or sets the embed URL for the embed block.
        /// </summary>
        [JsonPropertyName("embed")]
        public string? Embed { get; set; }

        /// <summary>
        /// Gets or sets the width of the embed block.
        /// </summary>
        [JsonPropertyName("width")]
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the embed block.
        /// </summary>
        [JsonPropertyName("height")]
        public int? Height { get; set; }
    }
}
