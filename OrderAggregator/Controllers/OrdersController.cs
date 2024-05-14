using System;
using Microsoft.AspNetCore.Mvc;
using OrderAggregator.Model;
using OrderAggregator.Services;

namespace OrderAggregator.Controllers;

/// <summary>
/// Controller that defines REST API methods for working with <see cref="Order"/>s.
/// </summary>
/// <param name="orderService"></param>
[Route("[controller]")]
[ApiController]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    /// <summary>
    /// Creates one or more <see cref="Order"/>s specified by the request.
    /// </summary>
    /// <param name="orders">Collection of orders to create.</param>
    /// <param name="cancellationToken">Cancellation token observed during request processing.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrders(IEnumerable<Order> orders, CancellationToken cancellationToken)
    {
        // Currently this endpoint is anonymous. This may or may not be desirable based on the actual use case.
        await _orderService.CreateOrdersAsync(orders, cancellationToken);

        return Created();
    }
}