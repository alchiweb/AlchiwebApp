using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Data model for attachment blocks, containing file information and an optional title.
    /// </summary>
    public class AttachesData : IBlockData
    {
        /// <summary>
        /// file information for the attachment.
        /// </summary>
        [JsonPropertyName("file")]
        public FileData? File { get; set; }
        /// <summary>
        /// title of the attachment.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }
    }
}
