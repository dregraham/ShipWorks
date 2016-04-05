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
    public class BrownDiscountedEdition : BrownEdition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BrownDiscountedEdition(StoreEntity store, BrownPostalAvailability postalAvailability)
            : base(store, postalAvailability)
        {
            foreach (ShipmentTypeCode shipmentType in Enum.GetValues(typeof(ShipmentTypeCode)))
            {
                if (new BrownEditionUtility().IsShipmentTypeAllowed(shipmentType))
                {
                    continue;
                }

                // For the Discounted version - we advertise Other, so in case the downloaded the wrong version, they know they can expand.
                if (shipmentType == ShipmentTypeCode.Other)
                {
                    AddRestriction(EditionFeature.ShipmentType, shipmentType, EditionRestrictionLevel.Forbidden);
                }
                else
                {
                    AddRestriction(EditionFeature.ShipmentType, shipmentType, EditionRestrictionLevel.Hidden);
                }
            }

            // Add account count restriction
            AddRestriction(EditionFeature.UpsAccountLimit, 1, EditionRestrictionLevel.Forbidden);
        }
    }
}
