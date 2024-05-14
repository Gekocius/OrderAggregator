using OrderAggregator.Model;

namespace OrderAggregator.Clients;

/// <summary>
/// Defines method for working with third party service.
/// <remarks>In production this would have more intelligent name, or would not exist at all (possibly replaced by HttpClient).</remarks>
/// </summary>
public interface IThirdPartySystemClient
{
    /// <summary>
    /// Sends given collection of <see cref="Order"/>s to a third party service.
    /// </summary>
    /// <param name="orders">Collection of orders to send.</param>
    /// <param name="cancellationToken">Cancellation token observed during send operation.</param>
    /// <returns></returns>
    public Task SendOrdersAsync(IEnumerable<Order> orders, CancellationToken cancellationToken);
}