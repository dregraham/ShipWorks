using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping
{
    [Component]
    public class ShipmentPrintHelperWrapper : IShipmentPrintHelper
    {
        public void InstallDefaultRules(ShipmentTypeCode shipmentType) =>
            ShipmentPrintHelper.InstallDefaultRules(shipmentType, false, null, true);
    }
}
