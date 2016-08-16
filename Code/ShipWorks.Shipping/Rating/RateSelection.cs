using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Rating
{
    /// <summary>
    /// Implementation of IRateSelection.
    /// </summary>
    public class RateSelection : IRateSelection
    {
        /// <summary>
        /// The integer value of the carrier specific service type
        /// </summary>
        public int ServiceType { get; set; }

        /// <summary>
        /// The integer value of the carrier specific confirmation type
        /// </summary>
        public int ConfirmationType { get; set; }
    }
}
