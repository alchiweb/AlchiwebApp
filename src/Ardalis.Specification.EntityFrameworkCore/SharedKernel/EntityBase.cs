namespace Ardalis.Specification.EntityFrameworkCore.SharedKernel;

/*
/// <summary>
/// A base class for DDD Entities. Includes support for domain events dispatched post-persistence.
/// If you prefer GUID Ids, change it here.
/// If you need to support both GUID and int IDs, change to EntityBase&lt;TId&gt; and use TId as the type for Id.
/// </summary>
public abstract class EntityBase : HasDomainEventsBase
{
  public Guid Id { get; set; }
}

public abstract class EntityBase<TId> : HasDomainEventsBase
  where TId : struct, IEquatable<TId>
{
  public TId Id { get; set; } = default!;
}
*/

/// <summary>
/// For use with Vogen or similar tools for generating code for 
/// strongly typed Ids.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TId"></typeparam>
public abstract class EntityBase<TEntity, TId> : HasDomainEventsBase, IEntityBase<TEntity, TId>
  where TEntity : EntityBase<TEntity, TId>
{
  public TId Id { get; init; }
  public EntityBase(TId id = default!)
  {
    Id = id;
  }
}
public abstract class EntityBase<TEntity> : HasDomainEventsBase, IEntityBase<TEntity, Guid>
  where TEntity : EntityBase<TEntity>
{
  public Guid Id {
    get;
    init => field = value == default ? Guid.CreateVersion7() : value;
  }
  public EntityBase(Guid id = default)
  {
    Id = id;
  }
}
