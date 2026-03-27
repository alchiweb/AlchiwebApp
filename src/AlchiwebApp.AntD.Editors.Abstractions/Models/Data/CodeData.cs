using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Code block data model.
    /// </summary>
    public class CodeData : IBlockData
    {
        /// <summary>
        /// Code content.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }
}
