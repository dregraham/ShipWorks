using ShipWorks.Core.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.ShippingPanel.Loading
{
    public interface IShipmentLoader
    {
        OrderSelectionLoaded Load(long orderID);
    }
}
