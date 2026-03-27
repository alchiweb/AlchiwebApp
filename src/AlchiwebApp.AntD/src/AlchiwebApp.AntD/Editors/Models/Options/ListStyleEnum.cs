using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Models.Options
{
    [JsonConverter(typeof(JsonStringEnumConverter<ListStyleEnum>))]
    public enum ListStyleEnum
    {
        [JsonStringEnumMemberName("unordered")]
        Unordered = 0,
        [JsonStringEnumMemberName("ordered")]
        Ordered,
        [JsonStringEnumMemberName("checklist")]
        Checklist
    }
}
