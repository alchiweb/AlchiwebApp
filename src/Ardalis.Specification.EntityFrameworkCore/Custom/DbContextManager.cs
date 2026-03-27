namespace Ardalis.Specification.EntityFrameworkCore.Custom;

public class DbContextManager<TContext> : IDbContextManager<TContext>, IDisposable, IAsyncDisposable
      where TContext : DbContext
{
    private readonly IDbContextFactory<TContext>? _dbContextFactory;
    private TContext? _dbContext;
    private TContext? _dbContextFromFactory;

    public bool IsDbFactory => _dbContextFactory != null;
    public TContext DbContext
    {
        get
        {
            if (_dbContextFactory == null)
            {
                if (_dbContext == null)
                    throw new Exception($"DbContext in DbContextManager is null");
                return _dbContext;
            }
            if (_dbContextFromFactory == null)
            {
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer // Use EF Core async methods rather than sync methods.
                _dbContextFromFactory = _dbContextFactory.CreateDbContext();
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer // Use EF Core async methods rather than sync methods.
            }
            return _dbContextFromFactory;
        } 
    }
    TContext IDbContextManager<TContext>.DbContext { get => DbContext; set => throw new NotImplementedException(); }

    public DbContextManager(IDbContextFactory<TContext>? dbContextFactory, TContext? dbContext)
    {
        _dbContextFactory = dbContextFactory;
        _dbContext = dbContext;
    }

    public DbSet<TEntity> GetDbSet<TEntity>()
        where TEntity : class
    {
        var dbSet = DbContext.Set<TEntity>();
        if (dbSet == null)
            throw new Exception($"DbSet for entity {typeof(TEntity).Name} is null");
        return dbSet;
    }
    public async ValueTask DisposeAsync()
    {
        if (_dbContextFromFactory is null)
            return;
        if (_dbContextFromFactory.ChangeTracker.HasChanges())
        {
            await _dbContextFromFactory.SaveChangesAsync();
        }
        await _dbContextFromFactory.DisposeAsync();
    }

    public void Dispose()
    {
        if (_dbContextFromFactory is null)
            return;
        if (_dbContextFromFactory.ChangeTracker.HasChanges())
        {
            _dbContextFromFactory.SaveChanges();
        }
        _dbContextFromFactory.Dispose();
    }
}
