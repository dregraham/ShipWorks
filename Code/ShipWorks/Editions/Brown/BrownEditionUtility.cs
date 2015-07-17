using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;

namespace ShipWorks.Editions.Brown
{
    /// <summary>
    /// Utility functions for working with the brown-only edition
    /// </summary>
    public static class BrownEditionUtility
    {
        /// <summary>
        /// Indicates if the shipment type is full allowed \ visible within brown-only edition
        /// </summary>
        public static bool IsShipmentTypeAllowed(ShipmentTypeCode shipmentType)
        {
            // UPS of course is allowed
            if (shipmentType == ShipmentTypeCode.UpsOnLineTools || shipmentType == ShipmentTypeCode.UpsWorldShip)
            {
                return true;
            }

            // We also allow these postal providers to be used.  There are additional restrictions that will kick-in that may limit if they can do APO\FPO, or all
            if (shipmentType == ShipmentTypeCode.Endicia || 
                shipmentType == ShipmentTypeCode.Express1Endicia ||
                shipmentType == ShipmentTypeCode.Express1Usps ||
                shipmentType == ShipmentTypeCode.Usps)
            {
                return true;
            }

            return false;
        }
    }
}
