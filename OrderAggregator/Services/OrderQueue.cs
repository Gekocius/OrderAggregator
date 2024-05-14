
using OrderAggregator.Model;

namespace OrderAggregator.Services;

/// <summary>
/// Stores and retrieves orders in a FIFO manner.
/// <remarks>In production this would most likely be a standalone service.</remarks>
/// </summary>
public class OrderQueue : IOrderQueue
{
    private readonly Queue<Order> _orderQueue = new();

    /// <inheritdoc/>
    public Task EnqueueOrderAsync(Order order, CancellationToken cancellationToken)
    {
        _orderQueue.Enqueue(order);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<Order?> DequeueOrderAsync(CancellationToken cancellationToken)
    {
        _orderQueue.TryDequeue(out var order);
        return Task.FromResult(order);
    }
}