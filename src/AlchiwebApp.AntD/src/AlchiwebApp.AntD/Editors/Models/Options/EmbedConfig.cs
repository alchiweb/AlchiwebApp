using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Models.Options
{
    /// <summary>
    /// Embed configuration options.
    /// </summary>
    public class EmbedConfig
    {
        /// <summary>
        /// Service configurations for embedding content.
        /// </summary>
        [JsonPropertyName("services")]
        public EmbedConfigServices? Services { get; set; }
    }
}
