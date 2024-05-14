using Microsoft.Extensions.Internal;
using OrderAggregator.Clients;
using OrderAggregator.Model;
using OrderAggregator.Services;

namespace OrderAggregator.Workers;

public class OrderAggregator(IOrderQueue orderQueue, TimeProvider timeProvider, IThirdPartySystemClient thirdPartySystemClient) : BackgroundService
{
    private readonly IOrderQueue _orderQueue = orderQueue;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IThirdPartySystemClient _thirdPartySystemClient = thirdPartySystemClient;

    private readonly Dictionary<string, Order> _aggregatedOrders = new(StringComparer.Ordinal);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Task.Yield() serves as a workaround for an issue with BackgroundService StartAsync method.
        // https://github.com/dotnet/runtime/issues/36063

        await Task.Yield();
        var lastSubmit = _timeProvider.GetUtcNow();
        while (!stoppingToken.IsCancellationRequested)
        {
            var order = await _orderQueue.DequeueOrderAsync(stoppingToken);
            UpsertOrder(order);

            if (_timeProvider.GetUtcNow() - lastSubmit >= TimeSpan.FromSeconds(20))
            {
                await SendOrders(stoppingToken);
                _aggregatedOrders.Clear();
                lastSubmit = _timeProvider.GetUtcNow();
            }
        }
    }

    private async Task SendOrders(CancellationToken stoppingToken)
    {
        if (_aggregatedOrders.Count == 0) return;

        await _thirdPartySystemClient.SendOrdersAsync(_aggregatedOrders.Values, stoppingToken);
    }

    private void UpsertOrder(Order? order)
    {
        if (order == null) return;

        if (_aggregatedOrders.ContainsKey(order.ProductId))
        {
            _aggregatedOrders[order.ProductId].Quantity += order.Quantity;
        }
        else
        {
            _aggregatedOrders.Add(order.ProductId, order);
        }
    }
}