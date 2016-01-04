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

        /// <summary>
        /// Refreshes the service types.
        /// </summary>
        void RefreshServiceTypes();

        /// <summary>
        /// Refreshes the package types.
        /// </summary>
        void RefreshPackageTypes();

        /// <summary>
        /// Are customs allowed for this shipment/shipment type?
        /// </summary>
        bool CustomsAllowed { get; set; }
    }
}