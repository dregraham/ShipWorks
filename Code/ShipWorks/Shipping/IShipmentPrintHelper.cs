using System.Windows.Forms;

namespace ShipWorks.Shipping
{
    public interface IShipmentPrintHelper
    {
        void InstallDefaultRules(ShipmentTypeCode shipmentType, bool reinstallMissing, IWin32Window owner);
    }
}