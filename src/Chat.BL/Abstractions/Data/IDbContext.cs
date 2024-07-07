using Microsoft.EntityFrameworkCore;

namespace Chat.BL.Abstractions.Data;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class;

    Task AddRangeAsync<TEntity>(params TEntity[] entities)
        where TEntity : class;

    Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class;

    void Remove<TEntity>(TEntity entity)
        where TEntity : class;

    void RemoveRange<TEntity>(params TEntity[] entities)
        where TEntity : class;

    void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class;

    void Update<TEntity>(TEntity entity)
        where TEntity : class;

    void UpdateRange<TEntity>(params TEntity[] entities)
        where TEntity : class;

    void UpdateRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class;
}