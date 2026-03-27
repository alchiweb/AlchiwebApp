using AlchiwebApp.AntD.Editors.Models;
using AlchiwebApp.AntD.Utils;
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors;

/// <summary>
/// Interop class for EditorJS JavaScript library.
/// </summary>
/// <param name="js_runtime"></param>
/// <param name="logger_factory"></param>
public sealed class EditorJsInterop(IJSRuntime js_runtime, ILoggerFactory logger_factory) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _module_task = new(() =>
            js_runtime.InvokeAsync<IJSObjectReference>("import", "./_content/AlchiwebApp.AntD/lib/editorjs-interop.js").AsTask());

    private readonly ILogger<EditorJsInterop> _logger = logger_factory.CreateLogger<EditorJsInterop>();

    private Lazy<DotNetObjectReference<EditorJsInterop>>? _dot_net_object_reference;
    private readonly Dictionary<string, Delegate> _update_delegates = [];
    /// <summary>
    /// Initialization timestamp in UTC.
    /// </summary>
    public readonly DateTime InitialisedUtcDateTime = DateTime.UtcNow;

    /// <summary>
    /// Dispose resources.
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        _dot_net_object_reference?.Value.Dispose();
        IJSObjectReference module = await _module_task.Value;
        await module.DisposeAsync();
    }

    private static string FormatElementSelectorKey(string id, string element_id) => $"{id}.{element_id}";

    /// <summary>
    /// Initialize the EditorJS instance.
    /// </summary>
    /// <param name="element_reference"></param>
    /// <param name="id"></param>
    /// <param name="jsobj"></param>
    /// <param name="tools"></param>
    /// <param name="configurations"></param>
    /// <param name="on_change"></param>
    /// <returns></returns>
    public async Task InitAsync(ElementReference element_reference, string id, JsonObject jsobj, ToolsConfigurations tools, EditorJsConfiguration configurations, Func<JsonObject, Task> on_change)
    {
        if (_dot_net_object_reference?.IsValueCreated is not true)
        {
            _dot_net_object_reference = new(() => DotNetObjectReference.Create(this));
        }

        string identifier = FormatElementSelectorKey(id, element_reference.Id);
        if (_update_delegates.ContainsKey(identifier))
        {
            return;
        }

        _update_delegates.Add(identifier, on_change);

        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.init", id, element_reference.Id, jsobj, JsonSerializer.SerializeToDocument(tools, EditorJsConfigurationJsonContext.Default.ToolsConfigurations), JsonSerializer.SerializeToDocument(configurations, EditorJsConfigurationJsonContext.Default.EditorJsConfiguration), _dot_net_object_reference.Value, nameof(OnChangeAsync));
    }

    /// <summary>
    /// Render content in the EditorJS instance.
    /// </summary>
    /// <param name="element_reference"></param>
    /// <param name="id"></param>
    /// <param name="jsobj"></param>
    /// <returns></returns>
    public async Task RenderAsync(ElementReference element_reference, string id, JsonObject jsobj)
    {
        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.render", id, element_reference.Id, jsobj);
    }

    /// <summary>
    /// Event invoked from JavaScript when content changes.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="element_id"></param>
    /// <param name="jsobj"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task<bool> OnChangeAsync(string id, string element_id, JsonObject jsobj)
    {
        string identifier = FormatElementSelectorKey(id, element_id);
        Delegate? update_delegate = _update_delegates.GetValueOrDefault(identifier);
        Task? invoked_delegate = update_delegate?.DynamicInvoke(jsobj) as Task;
        return await Task.FromResult(invoked_delegate?.IsCompletedSuccessfully ?? false);
    }
}
