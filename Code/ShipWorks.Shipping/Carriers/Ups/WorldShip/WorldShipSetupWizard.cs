using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Setup wizard for WorldShip
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypeSetupWizard), ShipmentTypeCode.UpsWorldShip)]
    public class WorldShipSetupWizard : UpsSetupWizard
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipSetupWizard(IShipmentTypeManager shipmentTypeManager) :
            base(ShipmentTypeCode.UpsWorldShip, false, shipmentTypeManager)
        {
        }
    }
}
