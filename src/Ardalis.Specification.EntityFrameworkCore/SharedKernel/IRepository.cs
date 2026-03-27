using System.Security.Cryptography;
using Ardalis.Specification;

namespace Ardalis.Specification.EntityFrameworkCore.SharedKernel;

/// <summary>
/// An abstraction for persistence, based on Ardalis.Specification
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TIdAssoc1"></typeparam>
/// <typeparam name="TIdAssoc2"></typeparam>
public interface IRepositoryAssociation<TEntity, TIdAssoc1, TIdAssoc2> : IRepositoryAssociationBase<TEntity, TIdAssoc1, TIdAssoc2>
  where TEntity : IEntityBase<TEntity>//, IAggregateRoot
  where TIdAssoc1 : struct, IEquatable<TIdAssoc1>
  where TIdAssoc2 : struct, IEquatable<TIdAssoc2>
{
}
/// <summary>
/// An abstraction for persistence, based on Ardalis.Specification
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepositoryAssociation<TEntity> : IRepositoryAssociationBase<TEntity, Guid, Guid>
  where TEntity : IEntityBase<TEntity>//, IAggregateRoot
{
}


/// <summary>
/// An abstraction for persistence, based on Ardalis.Specification
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IRepository<TEntity, TId> : IRepositoryBase<TEntity, TId>
  where TEntity : IEntityBase<TEntity, TId>//, IAggregateRoot
  where TId : struct, IEquatable<TId>
{
}

/// <summary>
/// Simplifier for Guid IDs
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> : IRepository<TEntity, Guid>
  where TEntity : IEntityBase<TEntity, Guid>//, IAggregateRoot
{

}
