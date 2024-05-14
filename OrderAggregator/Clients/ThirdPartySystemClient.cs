using System.Text.Json;
using OrderAggregator.Model;

namespace OrderAggregator.Clients;

/// <inheritdoc/>
public class ThirdPartySystemClient : IThirdPartySystemClient
{
    /// <inheritdoc/>
    public Task SendOrdersAsync(IEnumerable<Order> orders, CancellationToken cancellationToken)
    {
        Console.WriteLine(JsonSerializer.Serialize(orders));
        return Task.CompletedTask;
    }
}