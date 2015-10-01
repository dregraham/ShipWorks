namespace ShipWorks.Shipping.UI.ShippingPanel.Loading
{
    public interface IShipmentLoader
    {
        ShippingPanelLoadedShipment Load(long orderID);
    }
}
