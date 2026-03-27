using AlchiwebApp.AntD.Editors.Converters;

namespace AlchiwebApp.AntD.Editors;

/// <summary>
/// Service collection extensions for EditorJs related services.
/// </summary>
public static class EditorJsExtensions
{
    /// <summary>
    /// Parses the provided EditorJS JSON string and returns the rendered HTML string.
    /// </summary>
    /// <param name="EditorJsHtmlRenderer"></param>
    /// <param name="value"></param>
    /// <param name="strip_html"></param>
    /// <param name="styling_map"></param>
    /// <returns></returns>
    public static string Parse(this EditorJsHtmlRenderer EditorJsHtmlRenderer, string value, bool strip_html = false, string? styling_map = "[]")
        => EditorJsHtmlRenderer.ParseAsync(value, strip_html, styling_map).GetAwaiter().GetResult() ?? "";
}
