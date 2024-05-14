using OrderAggregator.Model;

namespace OrderAggregator.Services;

/// <summary>
/// Defines methods for queuing and retrieving <see cref="Order"/>s.
/// </summary>
public interface IOrderQueue
{
    /// <summary>
    /// Enqueues given order to message queue.
    /// </summary>
    /// <param name="order">Order to store.</param>
    public Task EnqueueOrderAsync(Order order, CancellationToken cancellationToken);

    /// <summary>
    /// Dequeues an <see cref="Order"/> from message queue.
    /// </summary>
    /// <param name="order">Order to store.</param>
    public Task<Order?> DequeueOrderAsync(CancellationToken cancellationToken);
}