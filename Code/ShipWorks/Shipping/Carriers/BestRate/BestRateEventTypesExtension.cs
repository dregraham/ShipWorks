using System;
using System.Linq;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public static class BestRateEventTypesExtension
    {
        /// <summary>
        /// Gets the latest best-rate event that occurred.
        /// </summary>
        /// <param name="events">The events.</param>
        public static BestRateEventTypes GetLatestBestRateEvent(this BestRateEventTypes events)
        {
            // Perform some bit masking on the event types of the shipment with all values of the 
            // enumeration to see which bits are set, then grab the max value to determine what
            // the latest event (latest event meaning the event that was closest to the shipment being
            // processed) that occurred).

            // Note: we can return the max value because of the order in which the values of the enum
            // are defined. Should additional values get added where the values are not in the order of
            // event furthest away from processing to event closest to processing, this logic will not
            // work and need to be adjusted.
            BestRateEventTypes latestEvent = Enum.GetValues(typeof(BestRateEventTypes)).Cast<BestRateEventTypes>()
                .Where(v => (v & events) == v)
                .Max();

            return latestEvent;
        }
    }
}
