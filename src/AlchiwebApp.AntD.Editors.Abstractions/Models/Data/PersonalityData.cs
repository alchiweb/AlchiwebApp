using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Personality block data model.
    /// </summary>
    public class PersonalityData : IBlockData
    {
        /// <summary>
        /// Name of the person.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        /// <summary>
        /// Description of the person.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        /// <summary>
        /// Link to more information about the person.
        /// </summary>
        [JsonPropertyName("link")]
        public string? Link { get; set; }
        /// <summary>
        /// Photo URL of the person.
        /// </summary>
        [JsonPropertyName("photo")]
        public string? Photo { get; set; }
    }
}
