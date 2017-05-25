using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Setup wizard for WorldShip
    /// </summary>
    [KeyedComponent(typeof(ShipmentTypeSetupWizardForm), ShipmentTypeCode.UpsWorldShip)]
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
