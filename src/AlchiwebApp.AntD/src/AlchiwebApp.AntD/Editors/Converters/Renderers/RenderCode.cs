using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;

namespace AlchiwebApp.AntD.Editors.Converters.Renderers;

/// <summary>
/// Code block renderer.
/// </summary>
public sealed class RenderCode : IBlockRenderer<CodeData>
{
    /// <summary>
    /// Renders the code block.
    /// </summary>
    /// <param name="render_tree_builder"></param>
    /// <param name="block"></param>
    public static void Render(EditorJsRenderTreeBuilder render_tree_builder, EditorJsBlock<CodeData> block)
    {
        string? code = block.Data?.Code;
        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "code");
        render_tree_builder.Builder.AddMarkupContent(render_tree_builder.SequenceCounter, code);
        render_tree_builder.Builder.CloseElement();
    }
}
