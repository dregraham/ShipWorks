using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Handle notification for Global Post labels
    /// </summary>
    public interface IGlobalPostLabelNotification
    {
        /// <summary>
        /// Show the notification and save result
        /// </summary>
        void Show(IShipmentEntity shipment);
    }
}