using AlchiwebApp.AntD.Editors.Abstractions.Models;

namespace AlchiwebApp.AntD.Editors.Converters.Renderers;

/// <summary>
/// Renderer CSS styles for the block.
/// </summary>
public sealed class BlockRenderCssStyle

{
    /// <summary>
    /// Builds the CSS styling for a given block type, id, and level from the provided render tree builder's styling map.
    /// </summary>
    /// <remarks>
    /// todo (2024-20-09|kibble) Add in optional parameters (default null) for things like header which matches on level for example...
    /// </remarks>
    /// <param name="render_tree_builder"></param>
    /// <param name="render_type"></param>
    /// <param name="id"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static EditorJsStylingMap? BuildEditorJsStylings(EditorJsRenderTreeBuilder render_tree_builder, BlockTypeEnum render_type, string? id, int? level = null)
    {
        EditorJsStylingMap? css = render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == render_type && item.Level == level && item.Id == id);
        css ??= render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == render_type && item.Level == level && item.Id == null);
        return css;
    }
}
