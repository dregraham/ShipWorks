using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Grid
{
    /// <summary>
    /// Defines who the feedabck is intended for
    /// </summary>
    public enum GridEbayFeedbackDirection
    {
        /// <summary>
        /// Recieved from the buyer
        /// </summary>
        Received,
        
        /// <summary>
        /// Left for the buyer
        /// </summary>
        Left
    }
}
