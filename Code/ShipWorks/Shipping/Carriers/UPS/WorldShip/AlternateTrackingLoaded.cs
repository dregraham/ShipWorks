using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Delegate for locating and reacting to an alternative UPS tracking method for special UPS services
    /// </summary>
    public delegate void AlternateTrackingLoaded(string alternateTrackingNumber, UpsContractService contractService, UpsServiceType upsServiceUsed);
}
