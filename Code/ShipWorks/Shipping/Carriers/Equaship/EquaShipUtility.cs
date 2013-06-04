using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.EquaShip.Enums;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Miscellaneous EquaShip functionality 
    /// </summary>
    public static class EquaShipUtility
    {
        /// <summary>
        /// Gets the valid services based on destination countryCode
        /// </summary>
        public static List<EquaShipServiceType> GetValidServiceTypes(string countryCode)
        {
            if (countryCode == "US")
            {
                return new List<EquaShipServiceType>
                {
                    EquaShipServiceType.Ground,
                    /* Currently unsupported Service types
                    EquaShipServiceType.Priority,
                    EquaShipServiceType.ExpressMail,
                    EquaShipServiceType.ExpressFlatRateEnvelope,
                    EquaShipServiceType.ExpressPriorityFlatRateEnvelope,
                    EquaShipServiceType.ExpressPriorityLargeFlatRateBox,
                    EquaShipServiceType.ExpressPriorityMediumFlatRateBox,
                    EquaShipServiceType.ExpressPrioritySmallFlatRateBox,
                    EquaShipServiceType.ExpressPriorityPaddedFlatRateEnvelope, */
                };
            }
            else
            {
                return new List<EquaShipServiceType>
                {
                    EquaShipServiceType.International
                };
            }
        }

        /// <summary>
        /// Determines if Confirmation services are available for the service/packaging combo provided
        /// </summary>
        public static bool IsConfirmationAvailable(EquaShipServiceType equashipServiceType, EquaShipPackageType equashipPackageType)
        {
            return true;
        }
    }
}
