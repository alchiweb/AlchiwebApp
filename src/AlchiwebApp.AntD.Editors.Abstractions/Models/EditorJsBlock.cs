using AlchiwebApp.AntD.Editors.Abstractions.JsonConverters;
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models
{
    /// <summary>
    /// Block model for EditorJS.
    /// </summary>
    [JsonConverter(typeof(EditorJsDataJsonConverter))]
    public class EditorJsBlock
    {
        /// <summary>
        /// Block type.
        /// </summary>
        [JsonPropertyName("type")]
        public required BlockTypeEnum Type { get; set; }
        /// <summary>
        /// Identifier of the block.
        /// </summary>
        [JsonPropertyName("id")]
        public required string Id { get; set; }
    }
    /// <summary>
    /// Block model for EditorJS with specific data type.
    /// </summary>
    /// <typeparam name="TBlockData"></typeparam>
    [JsonConverter(typeof(EditorJsDataJsonConverter))]
    public class EditorJsBlock<TBlockData> : EditorJsBlock where TBlockData : class, IBlockData
    {
        /// <summary>
        /// Data of the block.
        /// </summary>
        [JsonPropertyName("data")]
        public TBlockData? Data { get; set; }
    }


}