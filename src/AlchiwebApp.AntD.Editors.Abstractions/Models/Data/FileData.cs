using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// File block data model.
    /// </summary>
    public class FileData
    {
        /// <summary>
        /// URL of the file.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        /// <summary>
        /// Size of the file in bytes.
        /// </summary>
        [JsonPropertyName("size")]
        public int? Size { get; set; }
        /// <summary>
        /// Name of the file.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        /// <summary>
        /// Extension of the file.
        /// </summary>
        [JsonPropertyName("extension")]
        public string? Extension { get; set; }
    }
}
