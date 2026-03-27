using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Models.Options
{
    public class ListConfig
    {
        [JsonPropertyName("defaultStyle")]
        public ListStyleEnum DefaultStyle { get; set; } = ListStyleEnum.Unordered;
        [JsonPropertyName("maxLevel")]
        public int? MaxLevel { get; set; }
        [JsonPropertyName("counterTypes")]
        public ListCounterTypeEnum[]? CounterTypes { get; set; }
    }
}
