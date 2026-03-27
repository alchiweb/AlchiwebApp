using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;
using AlchiwebApp.AntD.Editors.Converters;

namespace AlchiwebApp.AntD.Editors.Renderers;

/// <summary>
/// Delimiter block renderer.
/// </summary>
public sealed class RenderDelimiter : IBlockRenderer<DelimiterData>
{
    /// <summary>
    /// Renders the delimiter block as an &lt;hr&gt; HTML element.
    /// </summary>
    /// <param name="render_tree_builder"></param>
    /// <param name="block"></param>
    public static void Render(EditorJsRenderTreeBuilder render_tree_builder, EditorJsBlock<DelimiterData> block)
    {
        string? id = block.Id;

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "hr");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "id", id);

        EditorJsStylingMap? css = render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == BlockTypeEnum.Delimiter && item.Id == id);
        css ??= render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == BlockTypeEnum.Delimiter && item.Id == null);

        if (css is not null)
        {
            render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "class", css.Style);
        }

        render_tree_builder.Builder.CloseElement();
    }
}
