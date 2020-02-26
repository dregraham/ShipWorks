using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.OneBalance;
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
        public WorldShipSetupWizard(IShipmentTypeManager shipmentTypeManager, Func<UpsAccountEntity, OneBalanceAccountAddressPage> oneBalanceAddressPageFactory) :
            base(ShipmentTypeCode.UpsWorldShip, false, shipmentTypeManager, oneBalanceAddressPageFactory)
        {
        }
    }
}
