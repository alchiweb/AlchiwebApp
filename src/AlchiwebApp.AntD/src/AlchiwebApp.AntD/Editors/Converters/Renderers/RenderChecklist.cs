using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;

namespace AlchiwebApp.AntD.Editors.Converters.Renderers;

/// <summary>
/// Checklist block renderer.
/// </summary>
public sealed class RenderChecklist : IBlockRenderer<ChecklistData>
{
    /// <summary>
    /// Renders the checklist block.
    /// </summary>
    /// <param name="render_tree_builder"></param>
    /// <param name="block"></param>
    public static void Render(EditorJsRenderTreeBuilder render_tree_builder, EditorJsBlock<ChecklistData> block)
    {
        string? id = block.Id;
        List<ChecklistItemData>? items = block.Data?.Items;

        if (items == null) { return; }

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "ul");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "id", id);
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "role", "group");
        render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "style", "list-style-type: none;");

        EditorJsStylingMap? css = render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == BlockTypeEnum.Checklist && item.Id == id);
        css ??= render_tree_builder.StylingMap.FirstOrDefault(item => item.Type == BlockTypeEnum.Checklist && item.Id == null);

        if (css is not null)
        {
            render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "class", css.Style);
        }

        foreach (var item in items)
        {
            render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "li");
            render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "aria-hidden", "true");

            if (css is not null && css.ItemStyle is not null)
            {
                render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "class", css.ItemStyle);
            }

            // Render the checkbox
            render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "input");
            render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "type", "checkbox");
            render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "disabled", "true");

            if (item.Checked ?? false)
            {
                render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "checked", "checked");
            }

            render_tree_builder.Builder.CloseElement(); // Close the input

            // Render checklist item text
            render_tree_builder.Builder.AddMarkupContent(render_tree_builder.SequenceCounter, item.Text);

            // Check and render nested checklists
            if (item.Items != null && item.Items.Count > 0)
            {
                RenderNestedCheckList(render_tree_builder, item.Items);
            }

            render_tree_builder.Builder.CloseElement(); // Close the li
        }

        render_tree_builder.Builder.CloseElement(); // Close the ul
    }

    private static void RenderNestedCheckList(EditorJsRenderTreeBuilder render_tree_builder, List<ChecklistItemData>? items)
    {
        if (items == null)
        {
            return;
        }

        render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "ul");

        foreach (var item in items)
        {
            render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "li");

            // Render the checkbox
            render_tree_builder.Builder.OpenElement(render_tree_builder.SequenceCounter, "input");
            render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "type", "checkbox");
            render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "disabled", "true");
            if (item.Checked ?? false)
            {
                render_tree_builder.Builder.AddAttribute(render_tree_builder.SequenceCounter, "checked", "checked");
            }

            render_tree_builder.Builder.CloseElement(); // Close the input

            // Render checklist item text
            render_tree_builder.Builder.AddMarkupContent(render_tree_builder.SequenceCounter, item.Text);

            // Check and render nested checklists
            if (item.Items != null && item.Items.Count > 0)
            {
                RenderNestedCheckList(render_tree_builder, item.Items);
            }

            render_tree_builder.Builder.CloseElement(); // Close the li
        }

        render_tree_builder.Builder.CloseElement(); // Close the ul
    }
}
