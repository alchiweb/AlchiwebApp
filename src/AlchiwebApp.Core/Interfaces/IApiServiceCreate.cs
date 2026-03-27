namespace AlchiwebApp.Core.Interfaces;

//public interface IApiServiceCreate<TEntityDto, TId>
//    where TEntityDto : IEntityDto<TId>
//    where TId : struct, IEquatable<TId>
//{
//    public Task<ApiResponse<TEntityDto>> Create(TEntityDto? dto);
//}
public interface IApiServiceCreate<TEntityDto, TId, TRequest>
    where TEntityDto : IEntityDto<TId>
    where TId : struct, IEquatable<TId>
    where TRequest : ICreateRequest<TEntityDto, TId>
{
    public Task<ApiResponse<TEntityDto>> Create(TRequest request, CancellationToken ct = default);
}
