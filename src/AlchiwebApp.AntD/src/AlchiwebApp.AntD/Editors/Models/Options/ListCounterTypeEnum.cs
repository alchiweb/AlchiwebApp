using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Models.Options
{
    [JsonConverter(typeof(JsonStringEnumConverter<ListCounterTypeEnum>))]
    public enum ListCounterTypeEnum
    {
        [JsonStringEnumMemberName("numeric")]
        Numeric = 0,
        [JsonStringEnumMemberName("upper-roman")]
        UpperRoman
    }
}
