namespace OrderAccumulator.Application.Contracts;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}
