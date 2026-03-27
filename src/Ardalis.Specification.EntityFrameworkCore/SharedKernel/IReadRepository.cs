using Ardalis.Specification;

namespace Ardalis.Specification.EntityFrameworkCore.SharedKernel;

/// <summary>
/// An abstraction for read only persistence operations, based on Ardalis.Specification.
/// Use this primarily to fetch trackable domain entities, not for custom queries.
/// </summary>
/// <typeparam name="TEntity">The type of entity being operated on by this repository.</typeparam>
/// <typeparam name="TId">The type of th Id's entity.</typeparam>
public interface IReadRepository<TEntity, TId> : IReadRepositoryBase<TEntity, TId>
  where TEntity : IEntityBase<TEntity, TId>, IAggregateRoot
  where TId : struct, IEquatable<TId>
{
}
