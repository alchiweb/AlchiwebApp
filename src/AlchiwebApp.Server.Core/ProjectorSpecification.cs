using Ardalis.Specification.EntityFrameworkCore.SharedKernel;
using Ardalis.Specification;
using AlchiwebApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using AlchiwebApp.Server.Core.Interfaces;

namespace AlchiwebApp.Server.Core;

public class ProjectorSpecification<TEntity, TEntityDto, TId, TService> : Specification<TEntity, TEntityDto>
    where TEntity : IEntityBase<TEntity, TId>
    where TEntityDto : IEntityDto<TId>
    where TId : struct, IEquatable<TId>
    where TService : IServerApiServiceBase<TEntity, TEntityDto, TId>
{
    public ProjectorSpecification(TService service)
    {
        var projector = service.GetMapToDtoMethod<TEntityDto>();
        Query.Select(entity => projector == null ? default! : projector.Invoke(entity));
    }
}
