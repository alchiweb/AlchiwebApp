using AlchiwebApp.Core.Interfaces;

namespace Alchiweb-App1.Core.AlchiwebApp;

public abstract record DtoBase<TId> : IEntityDto<TId> where TId : struct, IEquatable<TId>
{
    [Display(Name = "Id", ResourceType = typeof(I18n))]
    public TId Id { get; init; } = default;
}
