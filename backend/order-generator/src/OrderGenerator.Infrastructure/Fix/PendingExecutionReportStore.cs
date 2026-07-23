using System.Collections.Concurrent;
using QuickFix.FIX44;

namespace OrderGenerator.Infrastructure.Fix;

public sealed class PendingExecutionReportStore
{
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<ExecutionReport>> _pending = new();

    public Task<ExecutionReport> Register(Guid orderId, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<ExecutionReport>(TaskCreationOptions.RunContinuationsAsynchronously);

        if (!_pending.TryAdd(orderId, tcs))
        {
            throw new InvalidOperationException($"Order {orderId} is already pending.");
        }

        cancellationToken.Register(() =>
        {
            if (_pending.TryRemove(orderId, out var pending))
            {
                pending.TrySetCanceled(cancellationToken);
            }
        });

        return tcs.Task;
    }

    public void Complete(Guid orderId, ExecutionReport report)
    {
        if (_pending.TryRemove(orderId, out var pending))
        {
            pending.TrySetResult(report);
        }
    }

    public void Fail(Guid orderId, Exception exception)
    {
        if (_pending.TryRemove(orderId, out var pending))
        {
            pending.TrySetException(exception);
        }
    }
}