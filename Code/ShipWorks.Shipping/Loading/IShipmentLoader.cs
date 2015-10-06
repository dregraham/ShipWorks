using ShipWorks.Core.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.Loading
{
    public interface IShipmentLoader
    {
        OrderSelectionLoaded Load(long orderID);
    }
}
