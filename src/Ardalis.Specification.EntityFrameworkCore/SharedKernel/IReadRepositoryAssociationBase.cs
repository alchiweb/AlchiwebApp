using System.Collections.Generic;
using AlchiwebApp.PagingFiltering.Paging;
using Ardalis.Specification;

namespace Ardalis.Specification.EntityFrameworkCore.SharedKernel;


/// <summary>
/// <para>
/// A <see cref="IRepositoryBase{T}" /> can be used to query instances of <typeparamref name="TEntity" />.
/// An <see cref="ISpecification{T}"/> (or derived) is used to encapsulate the LINQ queries against the database.
/// </para>
/// </summary>
/// <typeparam name="TEntity">The type of entity being operated on by this repository.</typeparam>
/// <typeparam name="TId">The type of th Id's entity.</typeparam>
public interface IReadRepositoryBase<TEntity, TId>
  where TEntity : IEntityBase<TEntity, TId>
  where TId : struct, IEquatable<TId>
{
  IQueryable<TEntity> GetQueryWithoutSpecification(bool withTracking = true);

  /// <summary>
  /// Finds an entity with the given primary key value.
  /// </summary>
  /// <param name="id">The value of the primary key for the entity to be found.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
  /// </returns>
  Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
  /// </summary>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
  /// </returns>
  Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
  /// </summary>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains the <typeparamref name="TResult" />, or <see langword="null"/>.
  /// </returns>
  Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
  /// </returns>
  Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns the first entity that matches the specified criteria, projected to a result using the
  /// provided selector, or the default value if no entity is found.
  /// </summary>
  /// <remarks>If no entity matches the specification, the method returns the default value for the result type.
  /// The projector function is applied only to the first matching entity.</remarks>
  /// <typeparam name="TResult">The type of the result returned by the projector function.</typeparam>
  /// <param name="specification">The specification that defines the criteria used to filter entities.</param>
  /// <param name="projectorToDto">A function that projects an entity to the desired result type. If null, the method returns the default value for
  /// the result type.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the projected result of the first
  /// matching entity, or the default value for the result type if no entity matches the specification.</returns>
  Task<TResult?> FirstOrDefaultWithProjectorAsync<TResult>(ISpecification<TEntity> specification, Func<TEntity, TResult> projectorToDto, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns the first projected result that matches the specified criteria, or the default value if no
  /// match is found.
  /// </summary>
  /// <typeparam name="TResultEntity">The type of the entity returned by the specification before projection.</typeparam>
  /// <typeparam name="TResult">The type of the result after applying the projector function.</typeparam>
  /// <param name="specification">The specification that defines the criteria for selecting entities and the shape of the intermediate result.</param>
  /// <param name="projectorToDto">A function that projects the intermediate result entity to the desired result type. If null, the method returns
  /// the default value for <typeparamref name="TResult"/>.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the first projected result that
  /// matches the specification, or the default value for <typeparamref name="TResult"/> if no match is found.</returns>
  Task<TResult?> FirstOrDefaultWithProjectorAsync<TResultEntity, TResult>(ISpecification<TEntity, TResultEntity> specification, Func<TResultEntity, TResult> projectorToDto, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns the first entity projected to a result using the specified projector function, or the
  /// default value if no entities are found.
  /// </summary>
  /// <typeparam name="TResult">The type of the result returned by the projector function.</typeparam>
  /// <param name="projectorToDto">A function that projects an entity of type <typeparamref name="TEntity"/> to a result of type <typeparamref
  /// name="TResult"/>. If <see langword="null"/>, the method returns the default value for <typeparamref
  /// name="TResult"/>.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the first projected result, or the
  /// default value for <typeparamref name="TResult"/> if no entities are found.</returns>
  Task<TResult?> FirstOrDefaultWithProjectorAsync<TResult>(Func<TEntity, TResult> projectorToDto, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
  /// </summary>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
  /// </returns>
  Task<TEntity?> SingleOrDefaultAsync(ISingleResultSpecification<TEntity>? specification, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
  /// </summary>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains the <typeparamref name="TResult" />, or <see langword="null"/>.
  /// </returns>
  Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
  /// </returns>
  Task<TEntity?> SingleOrDefaultAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Finds all entities of <typeparamref name="TEntity" /> from the database.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
  /// </returns>
  Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
  /// <paramref name="specification"/>, from the database.
  /// </summary>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
  /// </returns>
  Task<List<TEntity>> ListAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default);

  /// <summary>
  /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
  /// <paramref name="specification"/>, from the database.
  /// <para>
  /// Projects each entity into a new form, being <typeparamref name="TResult" />.
  /// </para>
  /// </summary>
  /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains a <see cref="List{TResult}" /> that contains elements from the input sequence.
  /// </returns>
  Task<List<TResult>> ListAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);


  /// <summary>
  /// Finds all entities of <typeparamref name="TEntity" /> from the database.
  /// </summary>
  /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
  /// <param name="projectorToDto">The projector function to convert <typeparamref name="TEntity" /> to <typeparamref name="TResult" />.</param>
  /// <param name="skip">The number of elements to skip.</param>
  /// <param name="take">The number of elements to take.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
  /// </returns>
  /// 

  Task<List<TResult>> ListWithProjectorAsync<TResult>(Func<TEntity, TResult> projectorToDto, int? skip = null, int? take = null, CancellationToken cancellationToken = default);

  /// <summary>
  /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
  /// <paramref name="specification"/>, from the database.
  /// </summary>
  /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <param name="projectorToDto">The projector function to convert <typeparamref name="TEntity" /> to <typeparamref name="TResult" />.</param>
  /// <param name="take">The number of elements to take.</param>
  /// <param name="skip">The number of elements to skip.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation.
  /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
  /// </returns>
  Task<List<TResult>> ListWithProjectorAsync<TResult>(ISpecification<TEntity> specification, Func<TEntity, TResult> projectorToDto, int? skip = null, int? take = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// <para>
    /// Projects each entity into a new form, being <typeparamref name="TResult" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <typeparam name="TResultEntity">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="projectorToDto">The projector function to convert <typeparamref name="TResultEntity" /> to <typeparamref name="TResult" />.</param>
    /// <param name="skip">The number of elements to skip.</param>
    /// <param name="take">The number of elements to take.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{TResult}" /> that contains elements from the input sequence.
    /// </returns>
    Task<List<TResult>> ListWithProjectorAsync<TResult, TResultEntity>(ISpecification<TEntity, TResultEntity> specification, Func<TResultEntity, TResult> projectorToDto, int? skip = null, int? take = null, CancellationToken cancellationToken = default);

  Task<PagedList<TResult>> ProjectToPagedListAsync<TResult>(ISpecification<TEntity>? specification, Func<TEntity, TResult> projectorToDto, IBasePaging filter, CancellationToken cancellationToken);
  Task<PagedList<TResult>> ProjectToPagedListAsync<TResult, TDomainResult>(ISpecification<TEntity, TDomainResult> specification, Func<TDomainResult, TResult> projectorToDto, IBasePaging filter, CancellationToken cancellationToken);

  /// <summary>
  /// Returns a number that represents how many entities satisfy the encapsulated query logic
  /// of the <paramref name="specification"/>.
  /// </summary>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the
  /// number of elements in the input sequence.
  /// </returns>
  Task<int> CountAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns the total number of records.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the
  /// number of elements in the input sequence.
  /// </returns>
  Task<int> CountAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns a boolean that represents whether any entity satisfy the encapsulated query logic
  /// of the <paramref name="specification"/> or not.
  /// </summary>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains true if the 
  /// source sequence contains any elements; otherwise, false.
  /// </returns>
  Task<bool> AnyAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns a boolean whether any entity exists or not.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains true if the 
  /// source sequence contains any elements; otherwise, false.
  /// </returns>
  Task<bool> AnyAsync(CancellationToken cancellationToken = default);


#if NET8_0_OR_GREATER
  /// <summary>
  /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
  /// <paramref name="specification"/>, from the database.
  /// </summary>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <returns>
  ///  Returns an IAsyncEnumerable which can be enumerated asynchronously.
  /// </returns>
  IAsyncEnumerable<TEntity> AsAsyncEnumerable(ISpecification<TEntity> specification);
#endif
}


/// <summary>
/// <para>
/// A <see cref="IRepositoryBase{T}" /> can be used to query instances of <typeparamref name="TEntity" />.
/// An <see cref="ISpecification{T}"/> (or derived) is used to encapsulate the LINQ queries against the database.
/// </para>
/// </summary>
/// <typeparam name="TEntity">The type of entity being operated on by this repository.</typeparam>
/// <typeparam name="TIdAssoc1">The type of the Id1 entity.</typeparam>
/// <typeparam name="TIdAssoc2">The type of the Id2 entity.</typeparam>
public interface IReadRepositoryAssociationBase<TEntity, TIdAssoc1, TIdAssoc2>
  where TEntity : IEntityBase<TEntity>
  where TIdAssoc1 : struct, IEquatable<TIdAssoc1>
  where TIdAssoc2 : struct, IEquatable<TIdAssoc2>

{
    IQueryable<TEntity> GetQueryWithoutSpecification(bool withTracking = true);

    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    /// <param name="id1">The value of the primary key1 for the entity to be found.</param>
    /// <param name="id2">The value of the primary key2 for the entity to be found.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
    /// </returns>
    Task<TEntity?> GetByIdsAsync(TIdAssoc1 id1, TIdAssoc2 id2, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
    /// </returns>
    Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="TResult" />, or <see langword="null"/>.
    /// </returns>
    Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
    /// </returns>
    Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first entity that matches the specified criteria, projected to a result using the
    /// provided selector, or the default value if no entity is found.
    /// </summary>
    /// <remarks>If no entity matches the specification, the method returns the default value for the result type.
    /// The projector function is applied only to the first matching entity.</remarks>
    /// <typeparam name="TResult">The type of the result returned by the projector function.</typeparam>
    /// <param name="specification">The specification that defines the criteria used to filter entities.</param>
    /// <param name="projectorToDto">A function that projects an entity to the desired result type. If null, the method returns the default value for
    /// the result type.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the projected result of the first
    /// matching entity, or the default value for the result type if no entity matches the specification.</returns>
    Task<TResult?> FirstOrDefaultWithProjectorAsync<TResult>(ISpecification<TEntity> specification, Func<TEntity, TResult> projectorToDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first projected result that matches the specified criteria, or the default value if no
    /// match is found.
    /// </summary>
    /// <typeparam name="TResultEntity">The type of the entity returned by the specification before projection.</typeparam>
    /// <typeparam name="TResult">The type of the result after applying the projector function.</typeparam>
    /// <param name="specification">The specification that defines the criteria for selecting entities and the shape of the intermediate result.</param>
    /// <param name="projectorToDto">A function that projects the intermediate result entity to the desired result type. If null, the method returns
    /// the default value for <typeparamref name="TResult"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first projected result that
    /// matches the specification, or the default value for <typeparamref name="TResult"/> if no match is found.</returns>
    Task<TResult?> FirstOrDefaultWithProjectorAsync<TResultEntity, TResult>(ISpecification<TEntity, TResultEntity> specification, Func<TResultEntity, TResult> projectorToDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first entity projected to a result using the specified projector function, or the
    /// default value if no entities are found.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the projector function.</typeparam>
    /// <param name="projectorToDto">A function that projects an entity of type <typeparamref name="TEntity"/> to a result of type <typeparamref
    /// name="TResult"/>. If <see langword="null"/>, the method returns the default value for <typeparamref
    /// name="TResult"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first projected result, or the
    /// default value for <typeparamref name="TResult"/> if no entities are found.</returns>
    Task<TResult?> FirstOrDefaultWithProjectorAsync<TResult>(Func<TEntity, TResult> projectorToDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
    /// </returns>
    Task<TEntity?> SingleOrDefaultAsync(ISingleResultSpecification<TEntity>? specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="TResult" />, or <see langword="null"/>.
    /// </returns>
    Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
    /// </returns>
    Task<TEntity?> SingleOrDefaultAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="TEntity" /> from the database.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
    /// </returns>
    Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
    /// </returns>
    Task<List<TEntity>> ListAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// <para>
    /// Projects each entity into a new form, being <typeparamref name="TResult" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{TResult}" /> that contains elements from the input sequence.
    /// </returns>
    Task<List<TResult>> ListAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);


    /// <summary>
    /// Finds all entities of <typeparamref name="TEntity" /> from the database.
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="projectorToDto">The projector function to convert <typeparamref name="TEntity" /> to <typeparamref name="TResult" />.</param>
    /// <param name="skip">The number of elements to skip.</param>
    /// <param name="take">The number of elements to take.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
    /// </returns>
    /// 

    Task<List<TResult>> ListWithProjectorAsync<TResult>(Func<TEntity, TResult> projectorToDto, int? skip = null, int? take = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="projectorToDto">The projector function to convert <typeparamref name="TEntity" /> to <typeparamref name="TResult" />.</param>
    /// <param name="take">The number of elements to take.</param>
    /// <param name="skip">The number of elements to skip.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
    /// </returns>
    Task<List<TResult>> ListWithProjectorAsync<TResult>(ISpecification<TEntity> specification, Func<TEntity, TResult> projectorToDto, int? skip = null, int? take = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// <para>
    /// Projects each entity into a new form, being <typeparamref name="TResult" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <typeparam name="TResultEntity">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="projectorToDto">The projector function to convert <typeparamref name="TResultEntity" /> to <typeparamref name="TResult" />.</param>
    /// <param name="skip">The number of elements to skip.</param>
    /// <param name="take">The number of elements to take.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{TResult}" /> that contains elements from the input sequence.
    /// </returns>
    Task<List<TResult>> ListWithProjectorAsync<TResult, TResultEntity>(ISpecification<TEntity, TResultEntity> specification, Func<TResultEntity, TResult> projectorToDto, int? skip = null, int? take = null, CancellationToken cancellationToken = default);

    Task<PagedList<TResult>> ProjectToPagedListAsync<TResult>(ISpecification<TEntity>? specification, Func<TEntity, TResult> projectorToDto, IBasePaging filter, CancellationToken cancellationToken);
    Task<PagedList<TResult>> ProjectToPagedListAsync<TResult, TDomainResult>(ISpecification<TEntity, TDomainResult> specification, Func<TDomainResult, TResult> projectorToDto, IBasePaging filter, CancellationToken cancellationToken);

    /// <summary>
    /// Returns a number that represents how many entities satisfy the encapsulated query logic
    /// of the <paramref name="specification"/>.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// number of elements in the input sequence.
    /// </returns>
    Task<int> CountAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the total number of records.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// number of elements in the input sequence.
    /// </returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a boolean that represents whether any entity satisfy the encapsulated query logic
    /// of the <paramref name="specification"/> or not.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains true if the 
    /// source sequence contains any elements; otherwise, false.
    /// </returns>
    Task<bool> AnyAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a boolean whether any entity exists or not.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains true if the 
    /// source sequence contains any elements; otherwise, false.
    /// </returns>
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);


#if NET8_0_OR_GREATER
    /// <summary>
    /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>
    ///  Returns an IAsyncEnumerable which can be enumerated asynchronously.
    /// </returns>
    IAsyncEnumerable<TEntity> AsAsyncEnumerable(ISpecification<TEntity> specification);
#endif
}
