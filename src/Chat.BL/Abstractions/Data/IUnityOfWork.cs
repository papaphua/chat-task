using Microsoft.EntityFrameworkCore.Storage;

namespace Chat.BL.Abstractions.Data;

public interface IUnityOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}