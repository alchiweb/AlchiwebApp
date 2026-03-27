using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;

namespace AlchiwebApp.AntD.Editors.Converters.Renderers;

/// <summary>
/// Paragraph block renderer.
/// </summary>
public sealed class RenderParagraph : IBlockRenderer<ParagraphData>
{
    /// <summary>
    /// Renders the paragraph block as a &lt;p&gt; HTML element.
    /// </summary>
    /// <param name="render_tree_builder"></param>
    /// <param name="block"></param>
    public static void Render(EditorJsRenderTreeBuilder render_tree_builder, EditorJsBlock<ParagraphData> block)
    {
        string? id = block.Id;
        string? text = block.Data?.Text;

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "p");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "id", id);

        EditorJsStylingMap? css = render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == BlockTypeEnum.Paragraph && item.Id == id);
        css ??= render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == BlockTypeEnum.Paragraph && item.Id == null);

        if (css is not null)
        {
            render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "class", css.Style);
        }

        render_tree_builder.Builder.AddMarkupContent(render_tree_builder.SequenceCounter, text);
        render_tree_builder.Builder.CloseElement();
    }
}
