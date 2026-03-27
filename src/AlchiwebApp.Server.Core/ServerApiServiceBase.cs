using Ardalis.Specification.EntityFrameworkCore.SharedKernel;

using Ardalis.Specification;
using Microsoft.Extensions.Logging;
using AlchiwebApp.Core;
using AlchiwebApp.Core.Interfaces;
using AlchiwebApp.PagingFiltering.Paging;
using AlchiwebApp.Server.Core.Interfaces;

namespace AlchiwebApp.Server.Core;

public abstract class ServerApiServiceBase<TEntity, TEntityDto, TId, TService> : IServerApiServiceBase<TEntity, TEntityDto, TId>
    where TEntity : IEntityBase<TEntity, TId>
    where TEntityDto : IEntityDto<TId>
    where TService : IApiService<TEntityDto, TId>
    where TId : struct, IEquatable<TId>
{
    private readonly IRepository<TEntity, TId> _repository;
    protected readonly ILogger<TService> _logger;
    public ServerApiServiceBase(IRepository<TEntity, TId> repository, ILogger<TService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public abstract Func<TEntity, TResult>? GetMapToDtoMethod<TResult>() where TResult : TEntityDto;
    public abstract Func<TRequest, TEntity>? GetMapToEntityMethod<TRequest>();

    //public virtual async Task<ApiResponse<TEntityDto>> Create(TEntityDto? request) => await CreateGeneric(request);
    //public virtual async Task<ApiResponse<TEntityDto>> Create<TRequest>(TRequest? request) where TRequest : ICreateRequest<TEntityDto, TId> => await CreateGeneric(request);

    //public virtual async Task<ApiResponse<TEntityDto>> Update(TEntityDto request) => await UpdateGeneric(request.Id, request);
    //public virtual async Task<ApiResponse<TEntityDto>> Update<TRequest>(TRequest? request) where TRequest : IUpdateRequest<TEntityDto, TId> => await UpdateGeneric(request.Id, request);

    //public virtual async Task<ApiResponse> Delete(TId id) => await DeleteGeneric(id);

    //public virtual async Task<ApiResponse<TEntityDto>> GetById(TId id) => await GetByIdGeneric<TEntityDto>(id);

    //public virtual async Task<ApiResponse<PagedList<TEntityDto>>> List() => await ListGeneric<TEntityDto>();
    //public virtual async Task<ApiResponse<PagedList<TResult>>> List<TResult>() where TResult : TEntityDto => await ListGeneric<TResult>();

    //public virtual async Task<ApiResponse<TEntityDto>> First() => await FirstGeneric<TEntityDto>();

    //public virtual async Task<ApiResponse<TResult>> First<TResult>() where TResult : TEntityDto => await FirstGeneric<TResult>();


    //public virtual async Task<ApiResponse<TResult>> GetById<TResult>(TId id) where TResult : TEntityDto => await GetByIdGeneric<TResult>(id);


    protected async Task<ApiResponse<TEntityDto>> CreateGeneric<TRequest>(TRequest request, CancellationToken ct = default)
    {
        var response = new ApiResponse<TEntityDto>();
        if (request == null)
            return response;

        var mapToEntityMethod = GetMapToEntityMethod<TRequest>();
        if (mapToEntityMethod == null)
            return response;
        var entity = mapToEntityMethod.Invoke(request);

        entity = await _repository.AddAsync(entity);
        if (entity == null)
            return response;

        var mapToDtoMethod = GetMapToDtoMethod<TEntityDto>();
        if (mapToDtoMethod == null)
            return response;
        var dtoUpdated = mapToDtoMethod.Invoke(entity);
        if (dtoUpdated == null)
            return response;
        response.Result = dtoUpdated;

        return response;
    }
    protected async Task<ApiResponse<TEntityDto>> UpdateGeneric<TRequest>(TId id, TRequest request, CancellationToken ct = default)
    {
        var response = new ApiResponse<TEntityDto>();
        if (request == null)
            return response;

        var mapToEntityMethod = GetMapToEntityMethod<TRequest>();
        if (mapToEntityMethod == null)
            return response;
        var entity = mapToEntityMethod.Invoke(request);
        if (entity == null)
            return response;
        await _repository.UpdateAsync(entity);

        var mapToDtoMethod = GetMapToDtoMethod<TEntityDto>();
        if (mapToDtoMethod == null)
            return response;
        var dtoUpdated = mapToDtoMethod.Invoke(entity);
        if (dtoUpdated == null)
            return response;
        response.Result = dtoUpdated;

        return response;
    }
    protected async Task<ApiResponse<TResult>> FirstGeneric<TResult>(ISpecification<TEntity>? specification = null, CancellationToken ct = default) where TResult : TEntityDto
    {
        var response = new ApiResponse<TResult>();
        var entity = await _repository.FirstOrDefaultAsync(specification);
        if (entity == null)
            return response;

        //await _repository.UpdateAsync(request);

        var mapMethod = GetMapToDtoMethod<TResult>();
        if (mapMethod != null)
        {
            response.Result = mapMethod.Invoke(entity);
        }

        return response;
    }
    protected async Task<ApiResponse<TResult>> FirstGeneric<TResult>(ISpecification<TEntity, TEntity> specification, CancellationToken ct = default) where TResult : TEntityDto
    {
        var response = new ApiResponse<TResult>();
        var entity = await _repository.FirstOrDefaultAsync(specification);
        if (entity == null)
            return response;

        //await _repository.UpdateAsync(request);

        var mapMethod = GetMapToDtoMethod<TResult>();
        if (mapMethod != null)
        {
            response.Result = mapMethod.Invoke(entity);
        }

        return response;
    }
    protected async Task<ApiResponse<TResult>> SingleGeneric<TResult>(ISingleResultSpecification<TEntity>? specification = null, CancellationToken ct = default) where TResult : TEntityDto
    {
        var response = new ApiResponse<TResult>();
        var entity = await _repository.SingleOrDefaultAsync(specification);
        if (entity == null)
            return response;

        //await _repository.UpdateAsync(request);

        var mapMethod = GetMapToDtoMethod<TResult>();
        if (mapMethod != null)
        {
            response.Result = mapMethod.Invoke(entity);
        }

        return response;
    }
    protected async Task<ApiResponse<TResult>> SingleGeneric<TResult>(ISingleResultSpecification<TEntity, TEntity> specification, CancellationToken ct = default) where TResult : TEntityDto
    {
        var response = new ApiResponse<TResult>();
        var entity = await _repository.SingleOrDefaultAsync(specification);
        if (entity == null)
            return response;

        //await _repository.UpdateAsync(request);

        var mapMethod = GetMapToDtoMethod<TResult>();
        if (mapMethod != null)
        {
            response.Result = mapMethod.Invoke(entity);
        }

        return response;
    }


    protected async Task<ApiResponse<PagedList<TResult>>> ListGeneric<TResult>(IBasePaging filter, ISpecification<TEntity>? specification = null, CancellationToken ct = default) where TResult : TEntityDto
    {
        var response = new ApiResponse<PagedList<TResult>>();

        //if (filter?.Page != null && filter?.PageSize != null)
        //{
        //    var specPagination = new Specification<TEntity>().Query.Skip(filter.Page.Value).Take(filter.PageSize.Value);
        //    await ApplySpecification(specification);
        //}


        //specification = specification.Skip(f)
        //var data = await ApplySpecification(specification)
        //    .Skip(pagination.Skip)
        //    .Take(pagination.Take)

        var mapToDtoMethod = GetMapToDtoMethod<TResult>();
        if (mapToDtoMethod == null)
            return response;

        response.Result = await _repository.ProjectToPagedListAsync(specification, mapToDtoMethod, filter, ct);

        return response;
    }
    protected async Task<ApiResponse<PagedList<TResult>>> ListGeneric<TResult>(IBasePaging filter, ISpecification<TEntity, TEntity> specification, CancellationToken ct = default) where TResult : TEntityDto
    {
        var response = new ApiResponse<PagedList<TResult>>();

        //if (filter?.Page != null && filter?.PageSize != null)
        //{
        //    var specPagination = new Specification<TEntity>().Query.Skip(filter.Page.Value).Take(filter.PageSize.Value);
        //    await ApplySpecification(specification);
        //}


        //specification = specification.Skip(f)
        //var data = await ApplySpecification(specification)
        //    .Skip(pagination.Skip)
        //    .Take(pagination.Take)

        var mapToDtoMethod = GetMapToDtoMethod<TResult>();
        if (mapToDtoMethod == null)
            return response;

        response.Result = await _repository.ProjectToPagedListAsync(specification, mapToDtoMethod, filter, ct);

        return response;
    }
    protected async Task<ApiResponse<bool>> CheckIfExistGeneric(ISpecification<TEntity>? specification = null, CancellationToken ct = default)
    {
        var response = new ApiResponse<bool>
        {
            Result = await _repository.AnyAsync(specification)
        };
        return response;
    }
    protected async Task<ApiResponse<bool>> CheckIfExistGeneric(ISpecification<TEntity, TEntity> specification, CancellationToken ct = default)
    {
        var response = new ApiResponse<bool>
        {
            Result = await _repository.AnyAsync(specification)
        };
        return response;
    }
    protected async Task<ApiResponse> DeleteGeneric(TId id, CancellationToken ct = default)
    {
        var response = new ApiResponse();

        await _repository.DeleteByIdAsync(id);
        return response;

    }

    protected async Task<ApiResponse<TResult>> GetByIdGeneric<TResult>(TId id, CancellationToken ct = default) where TResult : TEntityDto
    {
        var response = new ApiResponse<TResult>();

        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return response;
        //return Exception TypedResults.NotFound();

        //await _repository.UpdateAsync(request);
        var mapMethod = GetMapToDtoMethod<TResult>();
        if (mapMethod != null)
        {
            response.Result = mapMethod.Invoke(entity);
        }
        return response;
    }
}
