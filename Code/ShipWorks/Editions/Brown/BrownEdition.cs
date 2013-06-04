using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Editions.Brown
{
    /// <summary>
    /// Base for BrownOnly editions
    /// </summary>
    public abstract class BrownEdition : Edition
    {
        BrownPostalAvailability postalAvailability;

        /// <summary>
        /// Concstructor
        /// </summary>
        protected BrownEdition(StoreEntity store, BrownPostalAvailability postalAvailability)
            : base(store)
        {
            this.postalAvailability = postalAvailability;

            // If no postal, remove all postal types
            if (postalAvailability == BrownPostalAvailability.None)
            {
                foreach (ShipmentTypeCode shipmentType in Enum.GetValues(typeof(ShipmentTypeCode)))
                {
                    if (PostalUtility.IsPostalShipmentType(shipmentType))
                    {
                        AddRestriction(EditionFeature.ShipmentType, shipmentType, EditionRestrictionLevel.Hidden);
                    }
                }
            }

            // Add restriction of postal services if necessary
            else if (postalAvailability == BrownPostalAvailability.ApoFpoPobox)
            {
                AddRestriction(EditionFeature.PostalApoFpoPoboxOnly, EditionRestrictionLevel.Forbidden);
            }
        }

        /// <summary>
        /// Get the postal restriction in affect
        /// </summary>
        public BrownPostalAvailability PostalAvailability
        {
            get { return postalAvailability; }
        }

        /// <summary>
        /// Default shipment type is UPS OLT
        /// </summary>
        public override ShipmentTypeCode? DefaultShipmentType
        {
            get
            {
                return ShipmentTypeCode.UpsOnLineTools;
            }
        }
    }
}
