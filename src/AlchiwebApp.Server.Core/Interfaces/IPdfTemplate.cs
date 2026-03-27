using Microsoft.AspNetCore.Components;
using AlchiwebApp.Core.Interfaces;

namespace AlchiwebApp.Server.Core.Interfaces;

public interface IPdfTemplate<T> : IComponent where T : IPdfTemplateModel
{
    public T Model { get; set; }
}
