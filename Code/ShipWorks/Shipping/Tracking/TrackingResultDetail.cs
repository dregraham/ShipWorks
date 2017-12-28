namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// A tracking detail line
    /// </summary>
    public class TrackingResultDetail
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TrackingResultDetail()
        {
        }

        /// <summary>
        /// The date the tracking event occurred
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// The time the tracking event ocurred
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// The location\city where it ocurred
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// What happened that cause the tracking event
        /// </summary>
        public string Activity { get; set; }
    }
}
