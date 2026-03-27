using Ardalis.Specification.EntityFrameworkCore.SharedKernel;
using AlchiwebApp.Core.Interfaces;

namespace AlchiwebApp.Server.Core.Interfaces;

public interface IServerApiServiceBase<TEntity, TEntityDto, TId> :
    IServerApiServiceBase
    where TEntity : IEntityBase<TEntity, TId>
    where TId : struct, IEquatable<TId>
    where TEntityDto : IEntityDto<TId>
{
    public abstract Func<TEntity, TResult>? GetMapToDtoMethod<TResult>() where TResult : TEntityDto;
    public abstract Func<TRequest, TEntity>? GetMapToEntityMethod<TRequest>();
}

public interface IServerApiServiceBase
{
}
