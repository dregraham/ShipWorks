using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// A class for obtaining a comma separated list of values for all of the 
    /// <see cref="BestRateEventTypes"/> descriptions that a <see cref="BestRateEventTypes"/> 
    /// instance may have (e.g. "Rates Compared", "Rate Selected", etc.).
    /// </summary>
    public class BestRateEventsDescription
    {
        private readonly BestRateEventTypes events;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateEventsDescription"/> class.
        /// </summary>
        /// <param name="events">The events.</param>
        public BestRateEventsDescription(BestRateEventTypes events)
        {
            this.events = events;
        }

        /// <summary>
        /// Returns a comma separated list of the corresponding <see cref="BestRateEventTypes"/> description
        /// based on the value of the <see cref="BestRateEventTypes"/> provided in the constructor.  
        /// </summary>
        public override string ToString()
        {
            List<string> descriptions = new List<string>();
            
            // We don't want None to appear in the list if there are multiple values of events, so 
            // build a comma separated list for all values that aren't None (since none has a value 
            // of 0, we need to explicitly filter it out of the list)
            List<BestRateEventTypes> eventTypes = Enum.GetValues(typeof(BestRateEventTypes)).OfType<BestRateEventTypes>().Where(e => e != BestRateEventTypes.None).ToList();

            foreach (BestRateEventTypes eventType in eventTypes)
            {
                if (events.HasFlag(eventType))
                {
                    // Our events value has the flag, so add it to the list of descriptions
                    descriptions.Add(EnumHelper.GetDescription(eventType));
                }
            }

            // If we have any descriptions in the list, we want to return those; an empty list of
            // descriptions means we should return the value for None (since BestRateEventTypes.None
            // was explicitly filtered out above)
            return descriptions.Any() ? 
                string.Join(", ", descriptions) : 
                EnumHelper.GetDescription(BestRateEventTypes.None);
        }
    }
}
