using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Holds data for the result of a tracking operation
    /// </summary>
    public class TrackingResult
    {
        string summary = "";

        List<TrackingResultDetail> details = new List<TrackingResultDetail>();

        /// <summary>
        /// Constructor
        /// </summary>
        public TrackingResult()
        {

        }

        /// <summary>
        /// The summary of the tracking infomration
        /// </summary>
        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }

        /// <summary>
        /// All the date\time and activity details
        /// </summary>
        public List<TrackingResultDetail> Details
        {
            get { return details; }
        }
    }
}
