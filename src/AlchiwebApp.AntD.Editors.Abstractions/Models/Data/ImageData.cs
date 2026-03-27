using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Image block data model.
    /// </summary>
    public class ImageData : IBlockData
    {
        /// <summary>
        /// URL of the image.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        /// <summary>
        /// File information for the image.
        /// </summary>
        [JsonPropertyName("file")]
        public FileData? File { get; set; }
        /// <summary>
        /// Gets or sets the caption for quote, image or embed blocks.
        /// </summary>
        [JsonPropertyName("caption")]
        public string? Caption { get; set; }
        /// <summary>
        /// Indicates whether the image should have a border.
        /// </summary>
        [JsonPropertyName("withBorder")]
        public bool WithBorder { get; set; } = false;
        /// <summary>
        /// Indicates whether the image should have a background.
        /// </summary>
        [JsonPropertyName("withBackground")]
        public bool WithBackground { get; set; } = false;
        /// <summary>
        /// Indicates whether the image should be stretched to fit the container.
        /// </summary>
        [JsonPropertyName("stretched")]
        public bool Stretched { get; set; } = false;
    }
}
