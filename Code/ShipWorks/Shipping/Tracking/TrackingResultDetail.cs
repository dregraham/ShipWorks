using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// A tracking detail line
    /// </summary>
    public class TrackingResultDetail
    {
        string date;
        string time;
        string location;
        string activity;

        /// <summary>
        /// Constructor
        /// </summary>
        public TrackingResultDetail()
        {

        }

        /// <summary>
        /// The date the tracking event occurred
        /// </summary>
        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// The time the tracking event ocurred
        /// </summary>
        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        /// <summary>
        /// The location\city where it ocurred
        /// </summary>
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        /// <summary>
        /// What happened that cause the tracking event
        /// </summary>
        public string Activity
        {
            get { return activity; }
            set { activity = value; }
        }
    }
}
