using System.ComponentModel.DataAnnotations;
using OrderAggregator.Model;

namespace OrderAggregator.Services;

/// <inheritdoc/>
public class OrderService(IOrderQueue orderQueue) : IOrderService
{
    private readonly IOrderQueue _orderQueue = orderQueue;

    /// <inheritdoc/>
    public async Task CreateOrdersAsync(IEnumerable<Order> orders, CancellationToken cancellationToken)
    {
        ValidateOrders(orders);

        foreach (var order in orders)
        {
            await _orderQueue.EnqueueOrderAsync(order, cancellationToken);
        }
    }

    private static void ValidateOrders(IEnumerable<Order> orders)
    {
        if (orders.Any(o => o.Quantity < 1))
        {
            throw new ValidationException("No order entry can have quantity lesser than 1.");
        }
    }
}