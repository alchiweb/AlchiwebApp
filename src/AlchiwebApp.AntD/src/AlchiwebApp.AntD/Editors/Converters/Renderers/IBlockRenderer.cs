using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;

namespace AlchiwebApp.AntD.Editors.Converters.Renderers;

/// <summary>
/// Interface for rendering a specific block type.
/// </summary>
/// <typeparam name="TBlockData"></typeparam>
public interface IBlockRenderer<TBlockData> where TBlockData : class, IBlockData
{
    /// <summary>
    /// Renders the specified block to the render tree builder.
    /// </summary>
    /// <param name="render_tree_builder"></param>
    /// <param name="block"></param>
    static abstract void Render(EditorJsRenderTreeBuilder render_tree_builder, EditorJsBlock<TBlockData> block);
}
