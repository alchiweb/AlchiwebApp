using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;
using AlchiwebApp.AntD.Editors.Abstractions.Utils;
using AlchiwebApp.AntD.Editors.Renderers;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace AlchiwebApp.AntD.Editors.Converters;

/// <summary>
/// A Razor component that converts JSON output from the EditorJS block editor into a Blazor RenderFragment.
/// </summary>
public partial class EditorJsRenderFragment : ComponentBase
{
    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public required RenderFragment ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the JSON output from the EditorJS block editor.
    /// </summary>
    /// <remarks>This JSON string is used to generate a segment of UI content.</remarks>
    [Parameter]
    public required string Value { get; set; }

    /// <summary>
    /// Gets or sets the JSON string that defines the styling map for the EditorJS blocks.
    /// </summary>
    /// <remarks>This JSON string is used to apply styles to the UI content.</remarks>
    [Parameter]
    public required string StylingMap { get; set; } = "[]";

    /// <summary>
    /// Gets or sets the logger instance used for logging within the component.
    /// </summary>
    [Inject]
    private ILogger<EditorJsRenderFragment> Logger { get; init; } = default!;

    /// <summary>
    /// Indicates whether the component's child's render fragment has been built.
    /// </summary>
    /// <remarks>
    /// Once the component has had it's render fragment built, it cannot be updated or amended at this time.
    /// </remarks>
    protected bool ChildRenderFragmentBuilt;

    /// <summary>
    /// Initializes the component and builds the child render fragment if it hasn't been built yet.
    /// </summary>
    protected override void OnInitialized()
        => BuildChildRenderFragment();

    /// <summary>
    /// Executed after the component has been rendered.
    /// </summary>
    /// <param name="first_render"></param>
    protected override void OnAfterRender(bool first_render)
        => BuildChildRenderFragment();

    private void BuildChildRenderFragment()
    {
        if (ChildRenderFragmentBuilt is true || string.IsNullOrWhiteSpace(Value))
        {
            return;
        }

        ChildContent = new(ConvertJsonToRenderFragment);
        StateHasChanged();
        ChildRenderFragmentBuilt = true;
    }

    /// <summary>
    /// Converts the JSON input into a RenderFragment for rendering in the Blazor component.
    /// </summary>
    private RenderFragment ConvertJsonToRenderFragment => builder =>
    {
        EditorJsData? blocks = null;
        IEnumerable<EditorJsStylingMap>? editor_js_styling_map;

        try
        {
            blocks = JsonSerializer.Deserialize(Value, typeof(EditorJsData), EditorJsJsonContext.Default) as EditorJsData;
            ArgumentNullException.ThrowIfNull(blocks, nameof(blocks));
        }
        catch (Exception ex)
        {
            Logger.LogError("Deserialise EditorJsBlocks Failed: {Exception}", ex.Message);
            throw;
        }

        try
        {
            editor_js_styling_map = JsonSerializer.Deserialize(StylingMap, typeof(IEnumerable<EditorJsStylingMap>), EditorJsJsonContext.Default) as IEnumerable<EditorJsStylingMap>;
            ArgumentNullException.ThrowIfNull(editor_js_styling_map, nameof(editor_js_styling_map));
        }
        catch (Exception ex)
        {
            Logger.LogError("Deserialise EditorJsStylingMap Failed: {Exception}", ex.Message);
            Logger.LogError("StylingMap: {StylingMap}", StylingMap);
            editor_js_styling_map = [];
        }

        try
        {
            EditorJsRenderTreeBuilder custom_render_tree_builder = new()
            {
                Builder = builder,
                StylingMap = editor_js_styling_map.ToList().AsReadOnly()
            };

            foreach (EditorJsBlock block in blocks.Blocks)
            {
                switch (block)
                {
                    case EditorJsBlock<CodeData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                    case EditorJsBlock<ParagraphData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                    case EditorJsBlock<HeaderData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                    case EditorJsBlock<ListData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                    case EditorJsBlock<QuoteData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                    case EditorJsBlock<ChecklistData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                    case EditorJsBlock<TableData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                    case EditorJsBlock<ImageData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                    case EditorJsBlock<DelimiterData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                    case EditorJsBlock<WarningData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                    case EditorJsBlock<EmbedData> specificBlock:
                        RenderBlock(custom_render_tree_builder, specificBlock);
                        break;
                        //case EditorJsBlock<TextData> specificBlock:
                        //    RenderBlock(custom_render_tree_builder, specificBlock);
                        //    break;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogTrace("RenderBlock Failed or was null value: {Exception}", ex.Message);
            return;
        }
    };

    /// <summary>
    /// Renders a single EditorJS block using the appropriate renderer based on the block type.
    /// </summary>
    /// <param name="render_tree_builder">The custom render tree builder used for rendering.</param>
    /// <param name="block">The EditorJS block to render.</param>
    internal static void RenderBlock(EditorJsRenderTreeBuilder render_tree_builder, EditorJsBlock block)
    {
        switch (block)
        {
            case EditorJsBlock<CodeData> specificBlock:
                RenderCode.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<ParagraphData> specificBlock:
                RenderParagraph.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<HeaderData> specificBlock:
                RenderHeader.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<ListData> specificBlock:
                RenderList.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<QuoteData> specificBlock:
                RenderQuote.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<ChecklistData> specificBlock:
                RenderChecklist.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<TableData> specificBlock:
                RenderTable.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<ImageData> specificBlock:
                RenderImage.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<DelimiterData> specificBlock:
                RenderDelimiter.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<WarningData> specificBlock:
                RenderWarning.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<EmbedData> specificBlock:
                RenderEmbed.Render(render_tree_builder, specificBlock);
                break;
            case EditorJsBlock<TextData> specificBlock:
                RenderText.Render(render_tree_builder, specificBlock);
                break;
                //case EditorJsBlock<TextData> specificBlock:
                //    RenderText.Render(render_tree_builder, specificBlock);
                //    break;
        }
    }
}
