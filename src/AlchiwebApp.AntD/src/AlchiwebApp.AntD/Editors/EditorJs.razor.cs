using AlchiwebApp.AntD.Editors;
using AntDesign;
using AntDesign.Internal;
using AlchiwebApp.AntD.Editors.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using AlchiwebApp.AntD.Editors.Converters;

namespace AlchiwebApp.AntD.Editors;

/// <summary>
/// EditorJs is a Blazor component that integrates the Editor.js rich text editor into a Blazor application.
/// </summary>
public partial class EditorJs : AntInputComponentBase<string>
{
    [Inject] private EditorJsHtmlRenderer EditorJsHtmlRenderer { get; init; } = default!;
    [Inject] private EditorJsInterop EditorJsInterop { get; init; } = default!;

    /// <summary>
    /// Event callback that is invoked when the value of the editor changes.
    /// </summary>
    [Parameter] public EventCallback<JsonObject> EditorValueChanged { get; set; }
    private JsonObject _editorValue = CreateEmptyJsonObject();
    /// <summary>
    /// Default value of the editor in JsonObject format.
    /// </summary>
    [Parameter]
    public JsonObject? DefaultEditorValue { get; set; }

    /// <summary>
    /// Value of the editor in JsonObject format.
    /// </summary>
    public JsonObject? EditorValue
    {
        get => _editorValue;
        set {
            if (value == null)
            {
                return;
            }

            var jsonString = value.ToJsonString() ?? "";
            if (string.Compare(jsonString, CurrentValue) == 0)
            {
                return;
            }

            _editorValue = value;
            CurrentValue = jsonString;
        }
    }
    /// <summary>
    /// Name attribute for the underlying HTML element.
    /// </summary>
    [Parameter] public string? Name { get; set; }
    /// <summary>
    /// Title attribute for the underlying HTML element.
    /// </summary>
    [Parameter] public string? Title { get; set; }
    /// <summary>
    /// Tool configurations for the Editor.js instance.
    /// </summary>
    [Parameter] public required ToolsConfigurations Tools { get; set; } = DefaultEditorToolsConfigurations;
    /// <summary>
    /// Configurations for the Editor.js instance.
    /// </summary>
    [Parameter] public required EditorJsConfiguration Configurations { get; set; } = new();

    /// <summary>
    /// Represents a reference to the rendered element.
    /// </summary>
    public ElementReference ElementReference;
    /// <summary>
    /// Gets the default JSON configurations for editor tools as a string.
    /// </summary>
    /// <remarks>
    /// This property provides access to the default JSON configurations for various editor tools.
    /// The string is maintained based on the contents of the 'libman.json' file and is intended
    /// to help newer developers get started quickly with default configurations.
    /// More detailed information about customizing tool options can be found in the README.md files.
    /// </remarks>
    public static ToolsConfigurations DefaultEditorToolsConfigurations => new()
    {
        Header = new(),
        LinkTool = new(),
        NestedList = new(),
        Marker = new(),
        Checklist = new(),
        CodeTool = new(),
        Delimiter = new(),
        SimpleImage = new(),
        Embed = new()
        {
            Options = new()
            {
                Config = new()
                {
                    Services = new()
                    {
                        Instagram = true,
                        YouTube = true,
                        Vimeo = true,
                        Imgur = true,
                        Twitter = true,
                        Facebook = true
                    }
                }
            }
        },
        InlineCode = new(),
        Quote = new(),
        Table = new()
    };

    private bool _should_render;

    /// <summary>
    /// Whether the component should render.
    /// </summary>
    /// <returns></returns>
    protected override bool ShouldRender() => _should_render;

    /// <summary>
    /// Provide prompt information that describes the expected value of the input field
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    //private string _inputString;

    private bool _afterFirstRender = false;


    /// <summary>
    /// Indicates that a page is being refreshed
    /// </summary>
    private string? _oldStyle;
    private bool _styleHasChanged;

    /// <summary>
    /// Initializes the component by setting up configurations.
    /// </summary>
    protected override void OnInitialized()
    {
        if (!string.IsNullOrWhiteSpace(Value))
        {
            _editorValue = JsonNode.Parse(Value)?.AsObject() ?? CreateEmptyJsonObject();
        }
        if (!string.IsNullOrEmpty(Placeholder))
        {
            Configurations.Placeholder = Placeholder;
        }
        if (string.IsNullOrEmpty(Style))
        {
            Style = "margin-top: 20px; border: thin dashed grey; padding: 0 20px 0 20px;";
        }
        base.OnInitialized();
    }

    /// <summary>
    /// Sets up the component's parameters and detects changes in the Style parameter.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        EditorValue = DefaultEditorValue;

        if (_oldStyle != Style)
        {
            _styleHasChanged = true;
            _oldStyle = Style;
        }
    }

    /// <summary>
    /// Event handler that is triggered when the current value changes.
    /// </summary>
    /// <param name="value"></param>
    protected override void OnCurrentValueChange(string value)
    {
        base.OnCurrentValueChange(value);
        //_inputString = value;
    }

    /// <summary>
    /// Event handler that is triggered after the component has been rendered.
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            await InitEditorJs();
            _afterFirstRender = true;
        }

        if (_afterFirstRender)
        {
            if (_styleHasChanged)
            {
                _styleHasChanged = false;
                if (!string.IsNullOrWhiteSpace(Style) && _afterFirstRender)
                {
                    await JsInvokeAsync(JSInteropConstants.StyleHelper.SetStyle, Ref, Style);
                }
            }
        }
        ///////////////////
        if (firstRender is true && _should_render is false)
        {
            _should_render = true;
            return;
        }
    }


    /// <summary>
    /// Initializes the Editor.js instance by invoking the necessary JavaScript interop methods.
    /// </summary>
    /// <returns></returns>
    public async Task InitEditorJs()
    {
        await EditorJsInterop.InitAsync(ElementReference, Id, EditorValue!, Tools, Configurations, OnContentChangedRequestAsync);
        if (_should_render)
        {
            _should_render = false;
        }
    }
    /// <summary>
    /// Handles changes made in the editorjs by updating the data model and invoking the user-defined event callback.
    /// </summary>
    /// <remarks>
    /// This method uses the <see cref="EqualityComparer{T}"/> to check for differences between the current and new values,
    /// in order to determine if the ValueChanged user-defined event callback needs to be invoked and
    /// the data model needs to be updated.
    /// </remarks>
    /// <param name="jsobj">The updated JSON object that represents the new value in the editorjs.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OnContentChangedRequestAsync(JsonObject jsobj)
    {
        await TryToChangeValue(jsobj);
    }

    /// <summary>
    /// Renders the editorjs with the new provided value, overriding the current value.
    /// This method uses <see cref="EqualityComparer{T}"/> to check if the new value is equal to the current value and if so, it will not execute the render function.
    /// </summary>
    /// <param name="jsobj">The new value to be rendered in the editorjs</param>
    /// <returns>A task that represents the asynchronous rendering operation.</returns>
    public async Task RenderAsync(JsonObject jsobj)
    {
        if (await TryToChangeValue(jsobj) == false)
        {
            return;
        }

        await EditorJsInterop.RenderAsync(ElementReference, Id, jsobj);
    }

    private async Task<bool> TryToChangeValue(JsonObject jsobj)
    {
        if (jsobj == null)
        {
            return false;
        }

        var oldCurrentValue = CurrentValue;
        // Try to change the value 
        EditorValue = jsobj;
        if (string.Compare(oldCurrentValue, CurrentValue) == 0)
        {
            return false;
        }

        await EditorValueChanged.InvokeAsync(jsobj);
        return true;
    }

    /// <summary>
    /// Provides a utility method for creating an empty <see cref="JsonObject"/>.
    /// </summary>
    /// <remarks>
    /// The method creates a new instance of <see cref="JsonObject"/> with a single property "blocks" which is an empty JSON array.
    /// </remarks>
    /// <returns>
    /// A new instance of <see cref="JsonObject"/> that represents an empty JSON object.
    /// </returns>
    public static JsonObject CreateEmptyJsonObject()
=> JsonNode.Parse("{\"blocks\": []}")!.AsObject();
}
