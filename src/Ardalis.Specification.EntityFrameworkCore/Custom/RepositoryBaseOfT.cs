using System.Security.Cryptography;
using AlchiwebApp.PagingFiltering.Paging;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification.EntityFrameworkCore.Custom.Exceptions;
using Ardalis.Specification.EntityFrameworkCore.SharedKernel;
using static System.Net.WebRequestMethods;

namespace Ardalis.Specification.EntityFrameworkCore.Custom;


public abstract class RepositoryBaseOfT<TContext> : IDisposable, IAsyncDisposable
  where TContext : DbContext
{
    protected TContext? _dbContext;
    private TContext? _oldDbContext;
    protected TContext? _dbContextFromFactory;
    protected readonly IDbContextFactory<TContext>? _dbContextFactory;

    public RepositoryBaseOfT(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory,
        TContext dbContext)
    {
        //_dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }
    public void CreateDbContextFromFactory()
    {
        if (_dbContextFactory == null)
            return;
        DeleteDbContextFromFactory();
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer // Use EF Core async methods rather than sync methods.
        _dbContextFromFactory = _dbContextFactory.CreateDbContext();
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer // Use EF Core async methods rather than sync methods.
        _oldDbContext = _dbContext;
        _dbContext = _dbContextFromFactory;
    }
    public async Task CreateDbContextFromFactoryAsync()
    {
        if (_dbContextFactory == null)
            return;
        await DeleteDbContextFromFactoryAsync();
        _dbContextFromFactory = await _dbContextFactory.CreateDbContextAsync();
        _oldDbContext = _dbContext;
        _dbContext = _dbContextFromFactory;
    }

    public void DeleteDbContextFromFactory()
    {
        if (_dbContextFromFactory is null)
            return;
        if (_dbContext is not null && _dbContext.ChangeTracker.HasChanges())
        {
            _dbContext.SaveChanges();
        }
        _dbContext = _oldDbContext;
        _dbContextFromFactory.Dispose();
        _dbContextFromFactory = null;
    }
    public async Task DeleteDbContextFromFactoryAsync()
    {
        if (_dbContextFromFactory is null)
            return;
        if (_dbContext is not null && _dbContext.ChangeTracker.HasChanges())
        {
            await _dbContext.SaveChangesAsync();
        }
        _dbContext = _oldDbContext;
        await _dbContextFromFactory.DisposeAsync();
        _dbContextFromFactory = null;
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await dbcm.DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(TContext dbContext, CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await DeleteDbContextFromFactoryAsync();
    }

    public void Dispose()
    {
        DeleteDbContextFromFactory();
    }
}
/*
public abstract class RepositoryBaseOfT<TEntity, TIdAssoc1, TIdAssoc2, TContext> : RepositoryBaseOfT<TContext>, IRepositoryAssociationBase<TEntity>, IDisposable, IAsyncDisposable
  where TEntity : class, IEntityBase<TEntity>
  where TIdAssoc1 : struct, IEquatable<TIdAssoc1>
  where TIdAssoc2 : struct, IEquatable<TIdAssoc2>
  where TContext : DbContext
{
    private readonly ISpecificationEvaluator _specificationEvaluator;

    public RepositoryBaseOfT(TContext dbContext)
      : this(dbContext, SpecificationEvaluator.Default)

    {
    }
    public RepositoryBaseOfT(TContext dbContext, ISpecificationEvaluator specificationEvaluator)
        : base(dbContext)

    {
        _specificationEvaluator = specificationEvaluator;
    }
    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory)
        : this(dbContextFactory, SpecificationEvaluator.Default)
    {
    }

    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, ISpecificationEvaluator specificationEvaluator)
        : base(dbContextFactory)
    {
        _specificationEvaluator = specificationEvaluator;
    }
    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, TContext dbContext)
      : this(dbContextFactory, dbContext, SpecificationEvaluator.Default)
    {
    }

    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, TContext dbContext, ISpecificationEvaluator specificationEvaluator)
        : base(dbContextFactory, dbContext)
    {
        _specificationEvaluator = specificationEvaluator;
    }
}
*/

public abstract class RepositoryBaseOfT<TEntity, TContext> : RepositoryBaseOfT<TContext>, IDisposable, IAsyncDisposable
where TEntity : class, IEntityBase<TEntity>
where TContext : DbContext
{
    private readonly ISpecificationEvaluator _specificationEvaluator;
    public RepositoryBaseOfT(TContext dbContext)
      : this(dbContext, SpecificationEvaluator.Default)

    {
    }
    public RepositoryBaseOfT(TContext dbContext, ISpecificationEvaluator specificationEvaluator)
        : base(dbContext)

    {
        _specificationEvaluator = specificationEvaluator;
    }
    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory)
        : this(dbContextFactory, SpecificationEvaluator.Default)
    {
    }

    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, ISpecificationEvaluator specificationEvaluator)
        : base(dbContextFactory)
    {
        _specificationEvaluator = specificationEvaluator;
    }
    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, TContext dbContext)
      : this(dbContextFactory, dbContext, SpecificationEvaluator.Default)
    {
    }

    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, TContext dbContext, ISpecificationEvaluator specificationEvaluator)
        : base(dbContextFactory, dbContext)
    {
        _specificationEvaluator = specificationEvaluator;
    }


    public IQueryable<TEntity> GetQueryWithoutSpecification(bool withTracking = true)
    {
        using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        DbSet<TEntity> dbSet = dbcm.GetDbSet<TEntity>();
        return (withTracking ? dbSet : dbSet.AsNoTracking()).AsQueryable();
    }

    ///// <inheritdoc/>
    //public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    //{
    //    await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
    //    return await dbcm.GetDbSet<TEntity>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    //}

    /// <inheritdoc/>
    public async Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default)
    {
        if (specification == null)
            return await FirstOrDefaultAsync(cancellationToken);
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await ApplySpecification(specification, dbcm.DbContext).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        if (specification.Selector == null)
        {
            if (specification is ISpecification<TEntity> specificationWithoutProjection)
            {
                var result = await FirstOrDefaultAsync(specificationWithoutProjection);
                if (result is TResult goodResult)
                    return goodResult;
            }
            throw new NullReferenceException("specification.Selector");
        }
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await ApplySpecification(specification, dbcm.DbContext).FirstOrDefaultAsync(cancellationToken);
    }
    /// <inheritdoc/>
    public async Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await dbcm.GetDbSet<TEntity>().FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult?> FirstOrDefaultWithProjectorAsync<TResult>(ISpecification<TEntity> specification, Func<TEntity, TResult> projectorToDto, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await ApplySpecification(specification, dbcm.DbContext).Select(entity => projectorToDto == null ? default! : projectorToDto.Invoke(entity)).FirstOrDefaultAsync(cancellationToken);
    }
    /// <inheritdoc/>
    public virtual async Task<TResult?> FirstOrDefaultWithProjectorAsync<TResultEntity, TResult>(ISpecification<TEntity, TResultEntity> specification, Func<TResultEntity, TResult> projectorToDto, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await ApplySpecification(specification, dbcm.DbContext).Select(entity => projectorToDto == null ? default! : projectorToDto.Invoke(entity)).FirstOrDefaultAsync(cancellationToken);
    }
    public virtual async Task<TResult?> FirstOrDefaultWithProjectorAsync<TResult>(Func<TEntity, TResult> projectorToDto, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await dbcm.GetDbSet<TEntity>().Select(entity => projectorToDto == null ? default! : projectorToDto.Invoke(entity)).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TEntity?> SingleOrDefaultAsync(ISingleResultSpecification<TEntity>? specification, CancellationToken cancellationToken = default)
    {
        if (specification == null)
            return await SingleOrDefaultAsync(cancellationToken);
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await ApplySpecification(specification, dbcm.DbContext).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<TEntity, TResult> specification,
      CancellationToken cancellationToken = default)
    {
        if (specification.Selector == null)
        {
            if (specification is ISingleResultSpecification<TEntity> specificationWithoutProjection)
            {
                var result = await SingleOrDefaultAsync(specificationWithoutProjection);
                if (result is TResult goodResult)
                    return goodResult;
            }
            throw new NullReferenceException("specification.Selector");
        }
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await ApplySpecification(specification, dbcm.DbContext).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TEntity?> SingleOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await dbcm.GetDbSet<TEntity>().FirstOrDefaultAsync(cancellationToken);
    }
    /// <inheritdoc/>
    public async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await dbcm.GetDbSet<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<TEntity>> ListAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default)
    {
        if (specification == null)
            return await ListAsync(cancellationToken);
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        var queryResult = await ApplySpecification(specification, dbcm.DbContext, withTracking: false).ToListAsync(cancellationToken);
        return specification.PostProcessingAction is null
            ? queryResult
            : specification.PostProcessingAction(queryResult).AsList();
    }
    public async Task<List<TResult>> ListAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        if (specification.Selector == null)
        {
            if (specification is ISpecification<TEntity> specificationWithoutProjection)
            {
                var result = await ListAsync(specificationWithoutProjection);
                if (result is List<TResult> goodResult)
                    return goodResult;
            }
            throw new NullReferenceException("specification.Selector");
        }
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        var queryResult = await ApplySpecification(specification, dbcm.DbContext, withTracking: false).ToListAsync(cancellationToken);
        return specification.PostProcessingAction is null
            ? queryResult
            : specification.PostProcessingAction(queryResult).AsList();
    }

    /// <inheritdoc/>
    public virtual async Task<List<TResult>> ListWithProjectorAsync<TResult>(Func<TEntity, TResult> projectorToDto, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        var query = dbcm.GetDbSet<TEntity>().AsNoTracking();
        if (skip.HasValue)
            query = query.Skip(skip.Value);
        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.Select(entity => projectorToDto == null ? default! : projectorToDto.Invoke(entity)).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<TResult>> ListWithProjectorAsync<TResult>(ISpecification<TEntity> specification, Func<TEntity, TResult> projectorToDto, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        var query = ApplySpecification(specification, dbcm.DbContext, withTracking: false);
        if (skip.HasValue)
            query = query.Skip(skip.Value);
        if (take.HasValue)
            query = query.Take(take.Value);

        return specification.PostProcessingAction is null
            ? await query.Select(entity => projectorToDto == null ? default! : projectorToDto.Invoke(entity)).ToListAsync(cancellationToken)
            : specification.PostProcessingAction(await query.ToListAsync(cancellationToken)).Select(entity => projectorToDto == null ? default! : projectorToDto.Invoke(entity)).AsList();
    }
    /// <inheritdoc/>
    public virtual async Task<List<TResult>> ListWithProjectorAsync<TResult, TResultEntity>(ISpecification<TEntity, TResultEntity> specification, Func<TResultEntity, TResult> projectorToDto, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        if (specification.Selector == null)
        {
            if (projectorToDto is Func<TEntity, TResult> projectorEntity)
                return await ListWithProjectorAsync(specification, projectorEntity, skip, take, cancellationToken);
            throw new NullReferenceException("specification.Selector");
        }
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        var query = ApplySpecification(specification, dbcm.DbContext, withTracking: false);
        if (skip.HasValue)
            query = query.Skip(skip.Value);
        if (take.HasValue)
            query = query.Take(take.Value);

        return specification.PostProcessingAction is null
            ? await query.Select(entity => projectorToDto == null ? default! : projectorToDto.Invoke(entity)).ToListAsync(cancellationToken)
            : specification.PostProcessingAction(await query.ToListAsync(cancellationToken)).Select(entity => projectorToDto == null ? default! : projectorToDto.Invoke(entity)).AsList();
    }
    public async Task<PagedList<TResult>> ProjectToPagedListAsync<TResult>(ISpecification<TEntity>? specification, Func<TEntity, TResult> projectorToDto, IBasePaging filter, CancellationToken cancellationToken)
    {
        int count = specification == null ?
            await CountAsync(cancellationToken) :
            await CountAsync(specification, cancellationToken);

        Pagination pagination = new(count, filter ?? new BasePaging() { Page = 1, PageSize = int.MaxValue });
        int? skip = null;
        int? take = null;
        if (filter?.Page != null || filter?.PageSize != null)
        {
            skip = pagination.Skip;
            take = pagination.Take;
        }
        List<TResult>? list = specification == null ?
            await ListWithProjectorAsync(projectorToDto, skip, take, cancellationToken) :
            await ListWithProjectorAsync(specification, projectorToDto, skip, take, cancellationToken);
        return new PagedList<TResult>(list, pagination);
    }
    public async Task<PagedList<TResult>> ProjectToPagedListAsync<TResult, TDomainResult>(ISpecification<TEntity, TDomainResult> specification, Func<TDomainResult, TResult> projectorToDto, IBasePaging filter, CancellationToken cancellationToken)
    {
        if (specification.Selector == null)
        {
            if (projectorToDto is Func<TEntity, TResult> projectorEntity)
                return await ProjectToPagedListAsync(specification, projectorEntity, filter, cancellationToken);
            throw new NullReferenceException("specification.Selector");
        }
        int count = await CountAsync(specification, cancellationToken);
        Pagination pagination = new(count, filter ?? new BasePaging() { Page = 1, PageSize = int.MaxValue });
        int? skip = null;
        int? take = null;
        if (filter?.Page != null || filter?.PageSize != null)
        {
            skip = pagination.Skip;
            take = pagination.Take;
        }
        List<TResult>? list = await ListWithProjectorAsync(specification, projectorToDto, skip, take, cancellationToken);
        return new PagedList<TResult>(list, pagination);
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await dbcm.GetDbSet<TEntity>().AsNoTracking().CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default)
    {
        if (specification == null)
            return await CountAsync(cancellationToken);
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await ApplySpecification(specification, dbcm.DbContext, true, withTracking: false).AsNoTracking().CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        if (specification.Selector == null)
        {
            if (specification is ISpecification<TEntity> specificationWithoutProjection)
            {
                return await CountAsync(specificationWithoutProjection);
            }
            throw new NullReferenceException("specification.Selector");
        }
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await ApplySpecification(specification, dbcm.DbContext, true, withTracking: false).AsNoTracking().CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await dbcm.GetDbSet<TEntity>().AnyAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> AnyAsync(ISpecification<TEntity>? specification, CancellationToken cancellationToken = default)
    {
        if (specification == null)
            return await AnyAsync(cancellationToken);
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await ApplySpecification(specification, dbcm.DbContext, true, withTracking: false).AsNoTracking().AnyAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> AnyAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        if (specification.Selector == null)
        {
            if (specification is ISpecification<TEntity> specificationWithoutProjection)
            {
                return await AnyAsync(specificationWithoutProjection);
            }
            throw new NullReferenceException("specification.Selector");
        }
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return await ApplySpecification(specification, dbcm.DbContext, true, withTracking: false).AsNoTracking().AnyAsync(cancellationToken);
    }
    /// <inheritdoc/>
    public IAsyncEnumerable<TEntity> AsAsyncEnumerable(ISpecification<TEntity> specification)
    {
        using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return ApplySpecification(specification, dbcm.DbContext).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        await dbcm.GetDbSet<TEntity>().AddAsync(entity, cancellationToken);

        await SaveChangesAsync(dbcm.DbContext, cancellationToken);
        return entity;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        await dbcm.GetDbSet<TEntity>().AddRangeAsync(entities);

        await SaveChangesAsync(dbcm.DbContext, cancellationToken);

        return entities;
    }

    /// <inheritdoc/>
    public async Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);

        var ids = GetIds(entity);
        var entityToUpdate = await GetByIdsAsync(dbcm, ids, cancellationToken);

        dbcm.DbContext.Entry(entityToUpdate).CurrentValues.SetValues(entity);

        return await SaveChangesAsync(dbcm.DbContext, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        dbcm.GetDbSet<TEntity>().UpdateRange(entities);

        return await SaveChangesAsync(dbcm.DbContext, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        dbcm.GetDbSet<TEntity>().Remove(entity);

        return await SaveChangesAsync(dbcm.DbContext, cancellationToken);
    }
    protected async Task<int> DeleteByIdsAsync(object?[]? ids, CancellationToken cancellationToken = default)
    {
        try
        {
            await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
            var entity = await GetByIdsAsync(dbcm, ids, cancellationToken);

            dbcm.GetDbSet<TEntity>().Remove(entity);

            return await dbcm.DbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Error deleting entity ({string.Join(",", ids?.Select(id => id?.ToString()) ?? [])})", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        dbcm.GetDbSet<TEntity>().RemoveRange(entities);

        var result = await SaveChangesAsync(dbcm.DbContext, cancellationToken);
        return result;
    }

    /// <inheritdoc/>
    public async Task<int> DeleteRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        await using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        var query = ApplySpecification(specification, dbcm.DbContext);
        dbcm.GetDbSet<TEntity>().RemoveRange(query);

        var result = await SaveChangesAsync(dbcm.DbContext, cancellationToken);
        return result;
    }

    //public async virtual Task<IQueryable<TEntity>> SkipAsync(int count)
    //{
    //  await using var dbContext = _dbContextFactory.CreateDbContext();
    //  return dbContext.Set<TEntity>().Skip(count);
    //}
    //public async virtual Task<IQueryable<TEntity>> TakeAsync(int count)
    //{
    //  await using var dbContext = _dbContextFactory.CreateDbContext();
    //  return dbContext.Set<TEntity>().Take(count);
    //}

    public virtual IQueryable<TEntity> Skip(int count)
    {
        using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return dbcm.GetDbSet<TEntity>().Skip(count);
    }
    public virtual IQueryable<TEntity> Take(int count)
    {
        using DbContextManager<TContext> dbcm = new(_dbContextFactory, _dbContext);
        return dbcm.GetDbSet<TEntity>().Take(count);
    }
    /*
    /// <summary>
    /// Filters the entities  of <typeparamref name="T"/>, to those that match the encapsulated query logic of the
    /// <paramref name="specification"/>.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="evaluateCriteriaOnly">It ignores pagination and evaluators that don't affect Count.</param>
    /// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
    public virtual IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification, bool evaluateCriteriaOnly = false, bool withTracking = true)
    {
      using var dbContext = _dbContextFactory.CreateDbContext();
      return ApplySpecification(specification, dbContext, evaluateCriteriaOnly, withTracking);
      //return _specificationEvaluator.GetQuery((withTracking ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking()).AsQueryable(), specification, evaluateCriteriaOnly);
    }

    /// <summary>
    /// Filters all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// <para>
    /// Projects each entity into a new form, being <typeparamref name="TResult" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
    public virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification, bool withTracking = true)
    {
      using var dbContext = _dbContextFactory.CreateDbContext();
      return ApplySpecification(specification, dbContext, withTracking);
      //return _specificationEvaluator.GetQuery((withTracking ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking()).AsQueryable(), specification);
    }
    */
    /// <summary>
    /// Filters the entities  of <typeparamref name="TEntity"/>, to those that match the encapsulated query logic of the
    /// <paramref name="specification"/>.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="evaluateCriteriaOnly">It ignores pagination and evaluators that don't affect Count.</param>
    /// <param name="withTracking"></param>
    /// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
    protected virtual IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification, TContext dbContext, bool evaluateCriteriaOnly = false, bool withTracking = true)
    {
        return _specificationEvaluator.GetQuery((withTracking ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking()).AsQueryable(), specification, evaluateCriteriaOnly);
    }

    /// <summary>
    /// Filters all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// <para>
    /// Projects each entity into a new form, being <typeparamref name="TResult" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="dbContext">The DbContext instance.</param>
    /// <param name="withTracking"></param>
    /// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification, TContext dbContext, bool withTracking = true)
    {
        return _specificationEvaluator.GetQuery((withTracking ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking()).AsQueryable(), specification);
    }
    /// <inheritdoc/>
    protected async Task<TEntity> GetByIdsAsync(DbContextManager<TContext> dbcm, object?[]? ids, CancellationToken cancellationToken = default)
    {
        return await dbcm.GetDbSet<TEntity>().FindAsync(ids, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException($"Error entity not found ({string.Join(",", ids?.Select(id => id?.ToString()) ?? [])})");
    }
    protected abstract object?[]? GetIds(TEntity entity);

}


public abstract class RepositoryBaseOfT<TEntity, TIdAssoc1, TIdAssoc2, TContext> : RepositoryBaseOfT<TEntity, TContext>, IRepositoryAssociationBase<TEntity, TIdAssoc1, TIdAssoc2>, IDisposable, IAsyncDisposable
  where TEntity : class, IEntityBase<TEntity>
  where TIdAssoc1 : struct, IEquatable<TIdAssoc1>
  where TIdAssoc2 : struct, IEquatable<TIdAssoc2>
  where TContext : DbContext
{
    public RepositoryBaseOfT(TContext dbContext)
      : base(dbContext)
    {
    }
    public RepositoryBaseOfT(TContext dbContext, ISpecificationEvaluator specificationEvaluator)
        : base(dbContext, specificationEvaluator)
    {
    }
    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, ISpecificationEvaluator specificationEvaluator)
        : base(dbContextFactory, specificationEvaluator)
    {
    }
    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, TContext dbContext)
      : base(dbContextFactory, dbContext)
    {
    }

    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, TContext dbContext, ISpecificationEvaluator specificationEvaluator)
        : base(dbContextFactory, dbContext, specificationEvaluator)
    {
    }


    /// <inheritdoc/>
    public async Task<TEntity?> GetByIdsAsync(TIdAssoc1 id1, TIdAssoc2 id2, CancellationToken cancellationToken = default)
        => await base.GetByIdsAsync(new(_dbContextFactory, _dbContext), [id1, id2], cancellationToken);

    public virtual async Task<int> DeleteByIdsAsync(TIdAssoc1 id1, TIdAssoc2 id2, CancellationToken cancellationToken = default)
       => await DeleteByIdsAsync([id1, id2], cancellationToken);

    //protected override object?[]? GetIds(TEntity entity) => [entity.Id];

}
public abstract class RepositoryBaseOfT<TEntity, TId, TContext> : RepositoryBaseOfT<TEntity, TContext>, IRepositoryBase<TEntity, TId>, IDisposable, IAsyncDisposable
  where TEntity : class, IEntityBase<TEntity, TId>
  where TId : struct, IEquatable<TId>
  where TContext : DbContext
{
    public RepositoryBaseOfT(TContext dbContext)
      : base(dbContext)
    {
    }
    public RepositoryBaseOfT(TContext dbContext, ISpecificationEvaluator specificationEvaluator)
        : base(dbContext, specificationEvaluator)
    {
    }
    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, ISpecificationEvaluator specificationEvaluator)
        : base(dbContextFactory, specificationEvaluator)
    {
    }
    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, TContext dbContext)
      : base(dbContextFactory, dbContext)
    {
    }

    public RepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory, TContext dbContext, ISpecificationEvaluator specificationEvaluator)
        : base(dbContextFactory, dbContext, specificationEvaluator)
    {
    }


    /// <inheritdoc/>
    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        => await base.GetByIdsAsync(new(_dbContextFactory, _dbContext), [id], cancellationToken);

    public virtual async Task<int> DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
       => await DeleteByIdsAsync([id], cancellationToken);

    protected override object?[]? GetIds(TEntity entity) => [entity.Id];

}
