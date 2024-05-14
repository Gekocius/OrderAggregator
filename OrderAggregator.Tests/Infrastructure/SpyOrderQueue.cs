using OrderAggregator.Model;
using OrderAggregator.Services;

namespace OrderAggregator.Tests.Infrastructure;

/// <summary>
/// Test double for <see cref="IOrderQueue"/>. Allows direct access to the messages sent to it.
/// </summary>
public class SpyOrderQueue : IOrderQueue
{
    public Queue<Order> InternalQueue { get; set; } = new();

    /// <inheritdoc/>
    public Task<Order?> DequeueOrderAsync(CancellationToken cancellationToken)
    {
        InternalQueue.TryDequeue(out var order);
        return Task.FromResult(order)!;
    }

    /// <inheritdoc/>
    public Task EnqueueOrderAsync(Order order, CancellationToken cancellationToken)
    {
        InternalQueue.Enqueue(order);

        return Task.CompletedTask;
    }
}