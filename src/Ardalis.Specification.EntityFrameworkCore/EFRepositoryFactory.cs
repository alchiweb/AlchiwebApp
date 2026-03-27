namespace Ardalis.Specification.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRepository">The Interface of the repository created by this Factory</typeparam>
/// <typeparam name="TConcreteRepository">
/// The Concrete implementation of the repository interface to create
/// </typeparam>
/// <typeparam name="TContext">The DbContext derived class to support the concrete repository</typeparam>
public class EFRepositoryFactory<TRepository, TConcreteRepository, TContext> : IRepositoryFactory<TRepository>
  where TConcreteRepository : TRepository
  where TContext : DbContext
{
    private readonly IDbContextFactory<TContext> _dbContextFactory;

    /// <summary>
    /// Initialises a new instance of the EFRepositoryFactory
    /// </summary>
    /// <param name="dbContextFactory">The IDbContextFactory to use to generate the TContext</param>
    public EFRepositoryFactory(IDbContextFactory<TContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public TRepository CreateRepository()
    {
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer // Use EF Core async methods rather than sync methods.
        var args = new object[] { _dbContextFactory.CreateDbContext() };
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer // Use EF Core async methods rather than sync methods.
        return (TRepository)Activator.CreateInstance(typeof(TConcreteRepository), args)!;
    }
}
