using System;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// Shipment view model
    /// </summary>
    public interface IShipmentViewModel : IDisposable
    {
        /// <summary>
        /// Load the view model with the given shipment adapter
        /// </summary>
        void Load(ICarrierShipmentAdapter shipmentAdapter);

        /// <summary>
        /// Save the contents of the view model into the loaded adapter
        /// </summary>
        void Save();
    }
}