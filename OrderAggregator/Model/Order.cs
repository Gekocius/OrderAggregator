namespace OrderAggregator.Model;

/// <summary>
/// Represents product order.
/// </summary>
public class Order
{
    /// <summary>
    /// Id of the ordered product.
    /// </summary>
    public required string ProductId { get; set; }

    /// <summary>
    /// Quantity of the ordered product. 
    /// </summary>
    public required int Quantity { get; set; }
}