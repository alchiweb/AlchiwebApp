using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models.Data
{
    /// <summary>
    /// Enum for quote alignment options.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuoteAlignmentEnum
    {
        /// <summary>
        /// Quote aligned to the left (default).
        /// </summary>
        [JsonStringEnumMemberName("left")]
        Left = 0,
        /// <summary>
        /// Quote aligned to the right.
        /// </summary>
        [JsonStringEnumMemberName("center")]
        Center
    }
}
