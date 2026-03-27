using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Models.Options
{
    public class TableConfig
    {
        [JsonPropertyName("rows")]
        public int? Rows { get; set; }
        [JsonPropertyName("cols")]
        public int? Cols { get; set; }
        [JsonPropertyName("maxRows")]
        public int? MaxRows { get; set; }
        [JsonPropertyName("maxCols")]
        public int? MaxCols { get; set; }
        [JsonPropertyName("withHeadings")]
        public bool? WithHeadings { get; set; }
        [JsonPropertyName("stretched")]
        public bool? Stretched { get; set; }
    }
}
