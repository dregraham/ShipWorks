using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for passing rate selection information without care to the specific carrier.
    /// </summary>
    public interface IRateSelection
    {
        /// <summary>
        /// The integer value of the carrier specific service type
        /// </summary>
        int ServiceType { get; set; }

        /// <summary>
        /// The integer value of the carrier specific confirmation type
        /// </summary>
        int ConfirmationType { get; set; }
    }
}
