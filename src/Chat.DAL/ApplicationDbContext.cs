using Chat.BL.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Chat.DAL;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IDbContext, IUnityOfWork
{
    public async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await Set<TEntity>()
            .AddAsync(entity);
    }

    public new void Remove<TEntity>(TEntity entity)
        where TEntity : class
    {
        Set<TEntity>()
            .Remove(entity);
    }

    public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        Set<TEntity>()
            .RemoveRange(entities);
    }

    public new void Update<TEntity>(TEntity entity)
        where TEntity : class
    {
        Set<TEntity>()
            .Update(entity);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}