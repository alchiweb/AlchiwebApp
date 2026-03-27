using AlchiwebApp.AntD.Editors.Abstractions.Models;

namespace AlchiwebApp.AntD.Editors.Converters.Models;

/// <summary>
/// Styling map for EditorJS blocks.
/// </summary>
public sealed class EditorJsStylingMap// : IEditorJsEntity<EditorJsStylingMap>
{
    //[JsonIgnore]
    //public static EditorJsStylingMap Empty => new()
    //{
    //    Type = SupportedRenderers.Empty,
    //    Style = ""
    //};

    /// <summary>
    /// Type of the block.
    /// </summary>
    [JsonPropertyName("type")]
    public required BlockTypeEnum Type { get; init; }

    /// <summary>
    /// Style associated with the block.
    /// </summary>
    [JsonPropertyName("style")]
    public required string Style { get; init; }

    /// <summary>
    /// Item style associated with the block.
    /// </summary>
    [JsonPropertyName("item-style")]
    public string? ItemStyle { get; init; }

    /// <summary>
    /// Footer style associated with the block.
    /// </summary>
    [JsonPropertyName("footer-style")]
    public string? FooterStyle { get; init; }

    /// <summary>
    /// ID of the block.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Level associated with the block.
    /// </summary>
    [JsonPropertyName("level")]
    public int? Level { get; init; }
}
