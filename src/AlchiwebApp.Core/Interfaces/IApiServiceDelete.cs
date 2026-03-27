namespace AlchiwebApp.Core.Interfaces;

public interface IApiServiceDelete<TEntityDto, TId>
    where TEntityDto : IEntityDto<TId>
    where TId : struct, IEquatable<TId>
{
    public Task<ApiResponse> Delete(TId id, CancellationToken ct = default);
}
