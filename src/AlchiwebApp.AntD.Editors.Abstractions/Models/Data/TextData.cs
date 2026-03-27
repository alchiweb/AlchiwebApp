using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Text block data model.
    /// </summary>
    public class TextData : IBlockData
    {
        /// <summary>
        /// Text content.
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
