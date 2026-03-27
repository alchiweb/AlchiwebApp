using Microsoft.AspNetCore.Http.HttpResults;
using AlchiwebApp.Core;
using AlchiwebApp.Core.Interfaces;
using AlchiwebApp.PagingFiltering.Paging;
using Microsoft.AspNetCore.Http;

namespace AlchiwebApp.Server.Core;

public static class RouteHelper
{

    /// <summary>
    /// Get list of items (get)
    /// </summary>
    /// <typeparam name="TApiService"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TDtoFilter"></typeparam>
    /// <param name="apiService"></param>
    /// <param name="filter"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<ApiResponse<PagedList<TEntityDto>>>, NotFound, InternalServerError>> GetListGeneric<TApiService, TEntityDto, TDtoFilter>(CancellationToken ct, TApiService apiService, TDtoFilter filter)
        where TApiService : IApiServiceList<TEntityDto, Guid, TDtoFilter>
        where TEntityDto : class, IEntityDto<Guid>
        where TDtoFilter : IBasePaging
    {
        try
        {
            var response = await apiService.List(filter);
            if (response?.Result == null)
                return TypedResults.NotFound();
            return TypedResults.Ok(response);
        }
        catch (Exception) { return TypedResults.InternalServerError(); }
    }

    /// <summary>
    /// Get list of items (get)
    /// </summary>
    /// <typeparam name="TApiService"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TDtoFilter"></typeparam>
    /// <typeparam name="TDtoWithEnum"></typeparam>
    /// <param name="apiService"></param>
    /// <param name="filter"></param>
    /// <param name="with"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<ApiResponse<PagedList<TEntityDto>>>, NotFound, InternalServerError>> GetListGeneric<TApiService, TEntityDto, TDtoFilter, TDtoWithEnum>(CancellationToken ct, TApiService apiService, TDtoFilter filter, TDtoWithEnum with)
        where TApiService : IApiServiceList<TEntityDto, Guid, TDtoFilter, TDtoWithEnum>
        where TEntityDto : class, IEntityDto<Guid>
        where TDtoFilter : IBasePaging
        where TDtoWithEnum : struct, Enum
    {
        try
        {
            var response = await apiService.List(filter, with);
            if (response?.Result == null)
                return TypedResults.NotFound();
            return TypedResults.Ok(response);
        }
        catch (Exception) { return TypedResults.InternalServerError(); }
    }

    /// <summary>
    /// Get an item (get) by id
    /// </summary>
    /// <typeparam name="TApiService"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <param name="apiService"></param>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<ApiResponse<TEntityDto>>, NotFound, InternalServerError>> GetByIdGeneric<TApiService, TEntityDto>(CancellationToken ct, TApiService apiService, Guid id)
        where TApiService : IApiServiceGet<TEntityDto, Guid>
        where TEntityDto : class, IEntityDto<Guid>
    {
        try
        {
            var response = await apiService.GetById(id);
            if (response?.Result == null)
                return TypedResults.NotFound();
            return TypedResults.Ok(response);
        }
        catch (Exception) { return TypedResults.InternalServerError(); }
    }


    /// <summary>
    /// Get an item (get) by id
    /// </summary>
    /// <typeparam name="TApiService"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TDtoWithEnum"></typeparam>
    /// <param name="apiService"></param>
    /// <param name="id"></param>
    /// <param name="with"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<ApiResponse<TEntityDto>>, NotFound, InternalServerError>> GetByIdGeneric<TApiService, TEntityDto, TDtoWithEnum>(CancellationToken ct, TApiService apiService, Guid id, TDtoWithEnum with)
        where TApiService : IApiServiceGet<TEntityDto, Guid, TDtoWithEnum>
        where TEntityDto : class, IEntityDto<Guid>
        where TDtoWithEnum : struct, Enum
    {
        try
        {
            var response = await apiService.GetById(id, with);
            if (response?.Result == null)
                return TypedResults.NotFound();
            return TypedResults.Ok(response);
        }
        catch (Exception) { return TypedResults.InternalServerError(); }
    }

    /// <summary>
    /// Create an item (post)
    /// </summary>
    /// <typeparam name="TApiService"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="apiService"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<ApiResponse<TEntityDto>>, NotFound, InternalServerError>> CreateGeneric<TApiService, TEntityDto, TRequest>(CancellationToken ct, TApiService apiService, TRequest request)
        where TApiService : IApiServiceCreate<TEntityDto, Guid, TRequest>
        where TEntityDto : class, IEntityDto<Guid>
        where TRequest : ICreateRequest<TEntityDto, Guid>
    {
        try
        {
            var response = await apiService.Create(request);
            if (response?.Result == null)
                return TypedResults.NotFound();
            return TypedResults.Ok(response);
        }
        catch (Exception) { return TypedResults.InternalServerError(); }
    }

    /// <summary>
    /// Update an item (put)
    /// </summary>
    /// <typeparam name="TApiService"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="apiService"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<ApiResponse<TEntityDto>>, NotFound, InternalServerError>> UpdateGeneric<TApiService, TEntityDto, TRequest>(CancellationToken ct, TApiService apiService, TRequest request)
        where TApiService : IApiServiceUpdate<TEntityDto, Guid, TRequest>
        where TEntityDto : class, IEntityDto<Guid>
        where TRequest : IRequestBase<Guid>, IUpdateRequest<TEntityDto, Guid>
    {
        try
        {
            var response = await apiService.Update(request);
            if (response?.Result == null)
                return TypedResults.NotFound();
            return TypedResults.Ok(response);
        }
        catch (Exception) { return TypedResults.InternalServerError(); }
    }

    /// <summary>
    /// Delete an item (delete)
    /// </summary>
    /// <typeparam name="TApiService"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <param name="apiService"></param>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static async Task<Results<Ok<ApiResponse>, NotFound, InternalServerError>> DeleteGeneric<TApiService, TEntityDto>(CancellationToken ct, TApiService apiService, Guid id)
        where TApiService : IApiServiceDelete<TEntityDto, Guid>
        where TEntityDto : class, IEntityDto<Guid>
    {
        try
        {
            var response = await apiService.Delete(id);
            if (response == null)
                return TypedResults.NotFound();
            return TypedResults.Ok(response);
        }
        catch (Exception) { return TypedResults.InternalServerError(); }
    }
}
