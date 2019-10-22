using ShipWorks.Stores.Orders.Split.Local;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Create an order splitter based on OrderSplitterType
    /// </summary>
    public interface IOrderSplitterFactory
    {
        IOrderSplitter Create(OrderSplitterType orderSplitterType);
    }
}
