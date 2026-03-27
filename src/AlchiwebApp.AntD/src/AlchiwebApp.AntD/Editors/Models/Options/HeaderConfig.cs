using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Models.Options
{
    /// <summary>
    /// Header configuration options.
    /// </summary>
    public class HeaderConfig
    {
        /// <summary>
        /// Placeholder text for the header input.
        /// </summary>
        [JsonPropertyName("placeholder")]
        public string? Placeholder { get; set; }
        /// <summary>
        /// Levels of headers available (e.g., [1, 2, 3] for H1, H2, H3).
        /// </summary>
        [JsonPropertyName("levels")]
        public int[]? Levels { get; set; }
        /// <summary>
        /// Default header level when the block is created.
        /// </summary>
        [JsonPropertyName("defaultLevel")]
        public int? DefaultLevel { get; set; }
    }
}
