using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Raw HTML block data model.
    /// </summary>
    public class RawData : IBlockData
    {
        /// <summary>
        /// HTML content.
        /// </summary>
        [JsonPropertyName("html")]
        public string? Html { get; set; }
    }
}
