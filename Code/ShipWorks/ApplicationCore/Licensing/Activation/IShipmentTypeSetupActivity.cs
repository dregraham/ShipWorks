using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.ApplicationCore.Licensing.Activation
{
    /// <summary>
    /// Activity for setting up a shipment type
    /// </summary>
    public interface IShipmentTypeSetupActivity
    {
        /// <summary>
        /// Initializes a shipment type, creating a default profile and optionally setting the shipment type as default 
        /// </summary>
        void InitializeShipmentType(ShipmentTypeCode shipmentTypeCode,
            ShipmentOriginSource origin,
            bool forceSetDefault = true,
            ThermalLanguage? requestedLabelFormat = null);
    }
}