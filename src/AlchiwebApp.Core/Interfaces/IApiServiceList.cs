using AlchiwebApp.PagingFiltering.Paging;

namespace AlchiwebApp.Core.Interfaces;

public interface IApiServiceList<TEntityDto, TId, TDtoFilter>
    where TEntityDto : IEntityDto<TId>
    where TId : struct, IEquatable<TId>
    where TDtoFilter : IBasePaging
{
    public Task<ApiResponse<PagedList<TEntityDto>>> List(TDtoFilter filter, CancellationToken ct = default);
}
public interface IApiServiceList<TEntityDto, TId, TDtoFilter, TWithEnum>
    where TEntityDto : IEntityDto<TId>
    where TId : struct, IEquatable<TId>
    where TDtoFilter : IBasePaging
    where TWithEnum : struct, Enum
{
    public Task<ApiResponse<PagedList<TEntityDto>>> List(TDtoFilter filter, TWithEnum with, CancellationToken ct = default);
}
