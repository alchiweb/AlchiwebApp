using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Warning block data model.
    /// </summary>
    public class WarningData : IBlockData
    {
        /// <summary>
        /// Title of the warning block.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        /// <summary>
        /// Message of the warning block.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
