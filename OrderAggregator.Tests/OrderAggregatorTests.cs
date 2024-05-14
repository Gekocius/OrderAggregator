using Microsoft.Extensions.Time.Testing;
using OrderAggregator.Clients;
using OrderAggregator.Model;
using OrderAggregator.Services;
using OrderAggregator.Tests.Infrastructure;

namespace OrderAggregator.Tests;

public class OrderAggregatorTests
{
    [Fact]
    public async Task SendsAggregatedOrdersUsingExternalServiceEveryTwentySeconds()
    {
        // Given
        var orders = new[] {
            new Order { ProductId = "dummyProductId1", Quantity = 1 },
            new Order { ProductId = "dummyProductId1", Quantity = 2 },
            new Order { ProductId = "dummyProductId2", Quantity = 1 },
            new Order { ProductId = "dummyProductId2", Quantity = 2 },
        };

        var expectedAggregatedOrders = new[] {
            new Order { ProductId = "dummyProductId1", Quantity = 3 },
            new Order { ProductId = "dummyProductId2", Quantity = 3 },
        };

        var spyOrderQueue = new SpyOrderQueue { InternalQueue = new Queue<Order>(orders) };
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 05, 14, 12, 00, 00, TimeSpan.FromHours(0)));
        var spyThirdPartySystemClient = new SpyThirdPartySystemClient();
        var orderAggregator = InstantiateOrderAggregator(spyOrderQueue, fakeTimeProvider, spyThirdPartySystemClient);

        // When
        using var cancellationTokenSource = new CancellationTokenSource();
        await orderAggregator.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(1000);
        fakeTimeProvider.Advance(TimeSpan.FromSeconds(20));
        await orderAggregator.StopAsync(cancellationTokenSource.Token);

        // Then
        Assert.Equivalent(expectedAggregatedOrders, spyThirdPartySystemClient.InternalStore);
    }

    private Workers.OrderAggregator InstantiateOrderAggregator(
        IOrderQueue? orderQueue = null,
        TimeProvider? timeProvider = null,
        IThirdPartySystemClient? thirdPartySystemClient = null)
    {
        return new Workers.OrderAggregator(
            orderQueue ?? new SpyOrderQueue(),
            timeProvider ?? new FakeTimeProvider(),
            thirdPartySystemClient ?? new SpyThirdPartySystemClient());
    }

    private class SpyThirdPartySystemClient : IThirdPartySystemClient
    {
        public List<Order> InternalStore { get; set; } = [];

        public Task SendOrdersAsync(IEnumerable<Order> orders, CancellationToken cancellationToken)
        {
            InternalStore.AddRange(orders);
            return Task.CompletedTask;
        }
    }
}