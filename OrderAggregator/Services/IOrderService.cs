using OrderAggregator.Model;

namespace OrderAggregator.Services;

/// <summary>
/// Defines methods for working with <see cref="Order"/>.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Creates new <see cref="Order"/>s from given data. 
    /// </summary>
    /// <param name="orders">Collection of orders to be created.</param>
    /// <param name="cancellationToken">Cancellation token observed during orders creation.</param>
    public Task CreateOrdersAsync(IEnumerable<Order> orders, CancellationToken cancellationToken);
}
