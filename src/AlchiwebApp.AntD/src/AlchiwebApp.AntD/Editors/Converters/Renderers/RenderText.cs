using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;

namespace AlchiwebApp.AntD.Editors.Converters.Renderers;

/// <summary>
/// Text block renderer.
/// </summary>
public sealed class RenderText : IBlockRenderer<TextData>
{
    /// <summary>
    /// Renders the text block.
    /// </summary>
    /// <param name="render_tree_builder"></param>
    /// <param name="block"></param>
    public static void Render(EditorJsRenderTreeBuilder render_tree_builder, EditorJsBlock<TextData> block)
    {
        string? text = block.Data?.Text;
        render_tree_builder.Builder.AddMarkupContent(render_tree_builder.SequenceCounter, text);
    }
}
