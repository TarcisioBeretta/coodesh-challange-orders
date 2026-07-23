using OrderAccumulator.Application.Contracts;

namespace OrderAccumulator.Infrastructure.Data.Repositories
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return context.SaveChangesAsync(cancellationToken);
        }
    }
}