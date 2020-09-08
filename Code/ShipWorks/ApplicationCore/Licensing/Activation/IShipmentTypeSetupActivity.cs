using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.ApplicationCore.Licensing.Activation
{
    public interface IShipmentTypeSetupActivity
    {
        void InitializeShipmentType(ShipmentTypeCode shipmentTypeCode,
            ShipmentOriginSource origin,
            bool forceSetDefault = true,
            ThermalLanguage? requestedLabelFormat = null);
    }
}