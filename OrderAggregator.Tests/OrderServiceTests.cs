using System.ComponentModel.DataAnnotations;
using OrderAggregator.Model;
using OrderAggregator.Services;
using OrderAggregator.Tests.Infrastructure;

namespace OrderAggregator.Tests;

public class OrderServiceTests
{
    [Fact]
    public async Task SendsSingleOrderFromGivenCollectionToMessageQueue()
    {
        // Given
        var expectedOrder = new Order
        {
            ProductId = "dummyProductId",
            Quantity = 1
        };

        var spyOrderQueue = new SpyOrderQueue();
        var orderService = InstantiateOrderService(spyOrderQueue);

        // When
        await orderService.CreateOrdersAsync([expectedOrder], CancellationToken.None);

        // Then
        Assert.Collection(spyOrderQueue.InternalQueue, o => AssertOrder(o, expectedOrder));
    }

    [Fact]
    public async Task SendsMultipleOrdersFromGivenCollectionToMessageQueue()
    {
        // Given
        var expectedOrder1 = new Order
        {
            ProductId = "dummyProductId",
            Quantity = 1
        };

        var expectedOrder2 = new Order
        {
            ProductId = "dummyProductId",
            Quantity = 1
        };

        var spyOrderQueue = new SpyOrderQueue();
        var orderService = InstantiateOrderService(spyOrderQueue);

        // When
        await orderService.CreateOrdersAsync([expectedOrder1, expectedOrder2], CancellationToken.None);

        // Then
        Assert.Collection(spyOrderQueue.InternalQueue, o => AssertOrder(o, expectedOrder1), o => AssertOrder(o, expectedOrder2));
    }

    [Fact]
    public async Task ThrowsValidationExceptionWhenAtLeastOneOrderHaveZeroQuantity()
    {
        // Given
        var expectedOrder1 = new Order
        {
            ProductId = "dummyProductId",
            Quantity = 0
        };

        var expectedOrder2 = new Order
        {
            ProductId = "dummyProductId",
            Quantity = 1
        };

        var spyOrderQueue = new SpyOrderQueue();
        var orderService = InstantiateOrderService(spyOrderQueue);

        // When
        var exception = await Record.ExceptionAsync(() => orderService.CreateOrdersAsync([expectedOrder1, expectedOrder2], CancellationToken.None));

        // Then
        Assert.NotNull(exception);
        Assert.IsType<ValidationException>(exception);
    }

    private static void AssertOrder(Order expectedOrder, Order order)
    {
        Assert.Equal(expectedOrder.ProductId, order.ProductId);
        Assert.Equal(expectedOrder.Quantity, order.Quantity);
    }

    private static IOrderService InstantiateOrderService(IOrderQueue? orderQueue = null)
    {
        return new OrderService(orderQueue ?? new SpyOrderQueue());
    }
}