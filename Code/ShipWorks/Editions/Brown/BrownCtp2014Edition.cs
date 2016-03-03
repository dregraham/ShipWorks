using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Editions.Brown
{
    /// <summary>
    /// Edition that represents UPS-Only version of ShipWorks
    /// </summary>
    public class BrownCtp2014Edition : BrownEdition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BrownCtp2014Edition(StoreEntity store, BrownPostalAvailability postalAvailability)
            : base(store, postalAvailability)
        {
            foreach (ShipmentTypeCode shipmentType in Enum.GetValues(typeof(ShipmentTypeCode)))
            {
                if ((new BrownEditionUtility()).IsShipmentTypeAllowed(shipmentType))
                {
                    continue;
                }

                // Explicitly allow other for the subsidized version.  It's not allowed for Discounted
                if (shipmentType == ShipmentTypeCode.Other)
                {
                    continue;
                }

                AddRestriction(EditionFeature.ShipmentType, shipmentType, EditionRestrictionLevel.Hidden);
            }
        }
    }
}
