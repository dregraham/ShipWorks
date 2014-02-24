using System;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Delegate for the RateSelected event
    /// </summary>
    public delegate void RateSelectedEventHandler(object sender, RateSelectedEventArgs e);

    /// <summary>
    /// EventArgs for the RateSelected event
    /// </summary>
    public class RateSelectedEventArgs : EventArgs
    {
        RateResult rate;

        /// <summary>
        /// Constructor
        /// </summary>
        public RateSelectedEventArgs(RateResult rate)
        {
            this.rate = rate;
        }

        /// <summary>
        /// The rate that was selec6ted
        /// </summary>
        public RateResult Rate
        {
            get { return rate; }
        }
    }
}
