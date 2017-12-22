using System.Collections.Generic;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Holds data for the result of a tracking operation
    /// </summary>
    public class TrackingResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TrackingResult()
        {
            Details = new List<TrackingResultDetail>();
        }

        /// <summary>
        /// The summary of the tracking infomration
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// All the date\time and activity details
        /// </summary>
        public List<TrackingResultDetail> Details { get; private set; }
    }
}
