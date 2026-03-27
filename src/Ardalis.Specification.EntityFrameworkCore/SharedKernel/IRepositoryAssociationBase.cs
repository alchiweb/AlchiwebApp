using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ardalis.Specification;

namespace Ardalis.Specification.EntityFrameworkCore.SharedKernel;

/// <summary>
/// <para>
/// A <see cref="IRepositoryBase{T}" /> can be used to query and save instances of <typeparamref name="TEntity" />.
/// An <see cref="ISpecification{T}"/> (or derived) is used to encapsulate the LINQ queries against the database.
/// </para>
/// </summary>
/// <typeparam name="TEntity">The type of entity being operated on by this repository.</typeparam>
/// <typeparam name="TId">The type of th Id's entity.</typeparam>
public interface IRepositoryBase<TEntity, TId> : IReadRepositoryBase<TEntity, TId>
  where TEntity : IEntityBase<TEntity, TId>
  where TId : struct, IEquatable<TId>
{
  /// <summary>
  /// Adds an entity in the database.
  /// </summary>
  /// <param name="entity">The entity to add.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains the <typeparamref name="TEntity" />.
  /// </returns>
  Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Adds the given entities in the database
  /// </summary>
  /// <param name="entities"></param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// </returns>
  Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an entity in the database
  /// </summary>
  /// <param name="entity">The entity to update.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
  Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates the given entities in the database
  /// </summary>
  /// <param name="entities">The entities to update.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
  Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

  /// <summary>
  /// Removes an entity in the database
  /// </summary>
  /// <param name="entity">The entity to delete.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
  Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an entity in the database
    /// </summary>
    /// <param name="id">Id of the entity to delete.</param>
    /// <param name="cancellationToken">Id of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);

  /// <summary>
  /// Removes the given entities in the database
  /// </summary>
  /// <param name="entities">The entities to remove.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
  Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

  /// <summary>
  /// Removes the all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
  /// <paramref name="specification"/>, from the database.
  /// </summary>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
  Task<int> DeleteRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

  /// <summary>
  /// Persists changes to the database.
  /// </summary>
  /// <returns>A task that represents the asynchronous operation.</returns>
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

  ///// <summary>
  ///// Filters the entities  of <typeparamref name="TEntity"/>, to those that match the encapsulated query logic of the
  ///// <paramref name="specification"/>.
  ///// </summary>
  ///// <param name="specification">The encapsulated query logic.</param>
  ///// <param name="evaluateCriteriaOnly">It ignores pagination and evaluators that don't affect Count.</param>
  ///// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
  //IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification, bool evaluateCriteriaOnly = false, bool withTracking = true);

  ///// <summary>
  ///// Filters all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
  ///// <paramref name="specification"/>, from the database.
  ///// <para>
  ///// Projects each entity into a new form, being <typeparamref name="TResult" />.
  ///// </para>
  ///// </summary>
  ///// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
  ///// <param name="specification">The encapsulated query logic.</param>
  ///// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
  //IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification, bool withTracking = true);

}

/// <summary>
/// <para>
/// A <see cref="IRepositoryBase{T}" /> can be used to query and save instances of <typeparamref name="TEntity" />.
/// An <see cref="ISpecification{T}"/> (or derived) is used to encapsulate the LINQ queries against the database.
/// </para>
/// </summary>
/// <typeparam name="TEntity">The type of entity being operated on by this repository.</typeparam>
/// <typeparam name="TIdAssoc1">The type of th Id's entity.</typeparam>
/// <typeparam name="TIdAssoc2">The type of th Id's entity.</typeparam>
public interface IRepositoryAssociationBase<TEntity, TIdAssoc1, TIdAssoc2> : IReadRepositoryAssociationBase<TEntity, TIdAssoc1, TIdAssoc2>
  where TEntity : IEntityBase<TEntity>
  where TIdAssoc1 : struct, IEquatable<TIdAssoc1>
  where TIdAssoc2 : struct, IEquatable<TIdAssoc2>
{
    /// <summary>
    /// Adds an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="TEntity" />.
    /// </returns>
    Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds the given entities in the database
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an entity in the database
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the given entities in the database
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an entity in the database
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an entity in the database
    /// </summary>
    /// <param name="id1">Id1 of the entity to delete.</param>
    /// <param name="id2">Id2 of the entity to delete.</param>
    /// <param name="cancellationToken">Id2 of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> DeleteByIdsAsync(TIdAssoc1 id1, TIdAssoc2 id2, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the given entities in the database
    /// </summary>
    /// <param name="entities">The entities to remove.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> DeleteRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists changes to the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Filters the entities  of <typeparamref name="TEntity"/>, to those that match the encapsulated query logic of the
    ///// <paramref name="specification"/>.
    ///// </summary>
    ///// <param name="specification">The encapsulated query logic.</param>
    ///// <param name="evaluateCriteriaOnly">It ignores pagination and evaluators that don't affect Count.</param>
    ///// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
    //IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification, bool evaluateCriteriaOnly = false, bool withTracking = true);

    ///// <summary>
    ///// Filters all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    ///// <paramref name="specification"/>, from the database.
    ///// <para>
    ///// Projects each entity into a new form, being <typeparamref name="TResult" />.
    ///// </para>
    ///// </summary>
    ///// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    ///// <param name="specification">The encapsulated query logic.</param>
    ///// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
    //IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification, bool withTracking = true);

}
