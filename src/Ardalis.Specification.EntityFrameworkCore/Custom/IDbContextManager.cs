using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.EntityFrameworkCore.Custom;

public interface IDbContextManager<TContext>
      where TContext : DbContext
{
    public bool IsDbFactory { get; }
    public TContext DbContext { get; set; }
    public DbSet<TEntity> GetDbSet<TEntity>()
                where TEntity : class;
}
