using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;

namespace AlchiwebApp.AntD.Editors.Converters.Renderers;
/// <summary>
/// Quote block renderer.
/// </summary>
public sealed class RenderQuote : IBlockRenderer<QuoteData>
{
    /// <summary>
    /// Renders the Quote block as a blockquote element.
    /// </summary>
    /// <param name="render_tree_builder"></param>
    /// <param name="block"></param>
    public static void Render(EditorJsRenderTreeBuilder render_tree_builder, EditorJsBlock<QuoteData> block)
    {
        string? id = block.Id;
        string? text = block?.Data?.Text;
        string? caption = block?.Data?.Caption;
        var alignment = block?.Data?.Alignment;

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "blockquote");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "id", id);

        EditorJsStylingMap? css = render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == BlockTypeEnum.Quote && item.Id == id);
        css ??= render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == BlockTypeEnum.Quote && item.Id == null);

        if (css is null)
        {
            if (alignment is not null)
            {
                render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "class", $"text-{alignment}");
            }
        }
        else
        {
            if (alignment is null)
            {
                render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "class", css.Style);
            }
            else
            {
                render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "class", $"text-{alignment} {css.Style}");
            }
        }

        render_tree_builder.Builder.AddMarkupContent(render_tree_builder.SequenceCounter, text);

        if (!string.IsNullOrEmpty(caption))
        {
            render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "footer");

            if (css is not null && css.FooterStyle is not null)
            {
                render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "class", css.FooterStyle);
            }

            render_tree_builder.Builder.AddMarkupContent(render_tree_builder.SequenceCounter, caption);
            render_tree_builder.Builder.CloseElement(); // Close the footer
        }

        render_tree_builder.Builder.CloseElement(); // Close the blockquote
    }
}
