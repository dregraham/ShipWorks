using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Speciailized identifier for MarketplaceAdvisor orders, which for OMS can have parcel numbers
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public class MarketplaceAdvisorOrderNumberIdentifier : OrderNumberIdentifier
    {
        int parcelNumber;

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOrderNumberIdentifier(long orderNumber)
            : this(orderNumber, 1)
        {

        }

        /// <summary>
        /// Constructor for orders that represent aditional parcels.  parcelNumber is 1-based.
        /// </summary>
        public MarketplaceAdvisorOrderNumberIdentifier(long orderNumber, int parcelNumber)
            : base(orderNumber)
        {
            if (parcelNumber < 1)
            {
                throw new ArgumentException("parcelNumber is 1-based", "parcelNumber");
            }

            this.parcelNumber = parcelNumber;
        }

        /// <summary>
        /// The parcel number for the order.  Will always be 1 except for additional parcels for OMS
        /// </summary>
        public int ParcelNumber
        {
            get { return parcelNumber; }
        }

        /// <summary>
        /// Apply the identifier to the given order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            base.ApplyTo(order);

            // Only need the postfix for additional parcels
            if (parcelNumber > 1)
            {
                order.ApplyOrderNumberPostfix(string.Format("-{0}", parcelNumber));
            }
        }
    }
}
