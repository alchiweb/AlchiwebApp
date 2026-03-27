namespace AlchiwebApp.AntD.Editors.Converters;

/// <summary>
/// Render tree builder context for EditorJs HTML rendering.
/// </summary>
public class EditorJsRenderTreeBuilder
{
    /// <summary>
    /// Styling map to be used during rendering.
    /// </summary>
    public required IReadOnlyList<EditorJsStylingMap> StylingMap { get; init; }
    /// <summary>
    /// Builder instance to construct the render tree.
    /// </summary>
    public required RenderTreeBuilder Builder { get; init; }

    /// <summary>
    /// By accessing the <see cref="SequenceCounter"/> it will automatically increment the value by one.
    /// </summary>
    public int SequenceCounter => _sequence_count++;
    private int _sequence_count = 0;

    /// <summary>
    /// Use this to get the current <see cref="SequenceCounter"/> without changing it's value
    /// </summary>
    public int GetSequenceCount => _sequence_count;
}
