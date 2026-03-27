using System.Security.Cryptography;

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
public interface IEntityBase<TEntity, TId> : IEntityBase<TEntity> // : IHasDomainEvents
  where TEntity : IEntityBase<TEntity, TId> //, IHasDomainEvents
{
  TId Id { get; }
}
/// <summary>
/// For use with Vogen or similar tools for generating code for 
/// strongly typed Ids.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEntityBase<TEntity> // : IHasDomainEvents
  where TEntity : IEntityBase<TEntity> //, IHasDomainEvents
{
}
