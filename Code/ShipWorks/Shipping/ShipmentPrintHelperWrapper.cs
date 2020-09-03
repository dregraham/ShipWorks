using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping
{
    [Component]
    public class ShipmentPrintHelperWrapper : IShipmentPrintHelper
    {
        public void InstallDefaultRules(ShipmentTypeCode shipmentType, bool reinstallMissing, IWin32Window owner) => ShipmentPrintHelper.InstallDefaultRules(shipmentType, reinstallMissing, owner);
    }
}
