namespace AlchiwebApp.Core.Interfaces;

public interface IUpdateRequest<TEntityDto, TId> where TEntityDto : IEntityDto<TId> where TId : struct, IEquatable<TId>
{
    TId Id { get; }
}