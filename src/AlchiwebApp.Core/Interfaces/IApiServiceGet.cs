namespace AlchiwebApp.Core.Interfaces;


public interface IApiServiceGet<TEntityDto, TId>
    where TEntityDto : IEntityDto<TId>
    where TId : struct, IEquatable<TId>
{
    public Task<ApiResponse<TEntityDto>> GetById(TId id, CancellationToken ct = default);
}
public interface IApiServiceGet<TEntityDto, TId, TWithEnum>
    where TEntityDto : IEntityDto<TId>
    where TId : struct, IEquatable<TId>
    where TWithEnum : struct, Enum
{
    public Task<ApiResponse<TEntityDto>> GetById(TId id, TWithEnum with, CancellationToken ct = default);
}
