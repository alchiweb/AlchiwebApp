namespace AlchiwebApp.Core.Interfaces;

//public interface IApiServiceUpdate<TEntityDto, TId>
//    where TEntityDto : IEntityDto<TId>
//    where TId : struct, IEquatable<TId>
//{
//    public Task<ApiResponse<TEntityDto>> Update(TEntityDto dto);
//}
public interface IApiServiceUpdate<TEntityDto, TId, TRequest>
    where TEntityDto : IEntityDto<TId>
    where TId : struct, IEquatable<TId>
    where TRequest : IRequestBase<Guid>, IUpdateRequest<TEntityDto, TId>
{
    public Task<ApiResponse<TEntityDto>> Update(TRequest request, CancellationToken ct = default);
}
