using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Abstractions.Models
{
    /// <summary>
    /// Block types supported by EditorJS.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<BlockTypeEnum>))]
    public enum BlockTypeEnum
    {
        /// <summary>
        /// Attaches
        /// </summary>
        [JsonStringEnumMemberName("attaches")]
        Attaches = 1,
        /// <summary>
        /// Checklist
        /// </summary>
        [JsonStringEnumMemberName("checklist")]
        Checklist,
        /// <summary>
        /// Code
        /// </summary>
        [JsonStringEnumMemberName("code")]
        Code,
        /// <summary>
        /// Delimiter
        /// </summary>
        [JsonStringEnumMemberName("delimiter")]
        Delimiter,
        /// <summary>
        /// Embed
        /// </summary>
        [JsonStringEnumMemberName("embed")]
        Embed,
        /// <summary>
        /// Header
        /// </summary>
        [JsonStringEnumMemberName("header")]
        Header,
        /// <summary>
        /// Image
        /// </summary>
        [JsonStringEnumMemberName("image")]
        Image,
        /// <summary>
        /// LinkTool
        /// </summary>
        [JsonStringEnumMemberName("linkTool")]
        LinkTool,
        /// <summary>
        /// List
        /// </summary>
        [JsonStringEnumMemberName("list")]
        List,
        /// <summary>
        /// Paragraph
        /// </summary>
        [JsonStringEnumMemberName("paragraph")]
        Paragraph,
        /// <summary>
        /// Personality
        /// </summary>
        [JsonStringEnumMemberName("personality")]
        Personality,
        /// <summary>
        /// Quote
        /// </summary>
        [JsonStringEnumMemberName("quote")]
        Quote,
        /// <summary>
        /// Raw
        /// </summary>
        [JsonStringEnumMemberName("raw")]
        Raw,
        /// <summary>
        /// Table
        /// </summary>
        [JsonStringEnumMemberName("table")]
        Table,
        /// <summary>
        /// Warning
        /// </summary>
        [JsonStringEnumMemberName("warning")]
        Warning,
        /// <summary>
        /// Text
        /// </summary>
        [JsonStringEnumMemberName("text")]
        Text,
    }
}