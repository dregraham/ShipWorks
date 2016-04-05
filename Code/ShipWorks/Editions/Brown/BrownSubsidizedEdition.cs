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
    public class BrownSubsidizedEdition : BrownEdition
    {
        List<string> upsAccounts;

        /// <summary>
        /// Create a new instance of the subsidized edition that is restricted to the given account list
        /// </summary>
        public BrownSubsidizedEdition(StoreEntity store, IEnumerable<string> accounts, BrownPostalAvailability postalAvailability)
            : base(store, postalAvailability)
        {
            upsAccounts = accounts.ToList();

            // Add shipment type restrictions
            foreach (ShipmentTypeCode shipmentType in Enum.GetValues(typeof(ShipmentTypeCode)))
            {
                if (new BrownEditionUtility().IsShipmentTypeAllowed(shipmentType))
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

            // Add account count restriction
            AddRestriction(EditionFeature.UpsAccountLimit, upsAccounts.Count, EditionRestrictionLevel.Forbidden);
            AddRestriction(EditionFeature.UpsAccountNumbers, upsAccounts, EditionRestrictionLevel.Forbidden);
        }

        /// <summary>
        /// Expose the list of accounts supported by this subsidized edition
        /// </summary>
        public IEnumerable<string> UpsAccounts
        {
            get { return upsAccounts; }
        }
    }
}
