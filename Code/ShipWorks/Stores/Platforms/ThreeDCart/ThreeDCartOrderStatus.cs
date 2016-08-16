using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Helper class that represents an online 3dcart order status
    /// </summary>
    public class ThreeDCartOrderStatus
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statusID">The 3dcart order Status ID</param>
        /// <param name="statusText">The 3dcart order Status Text</param>
        public ThreeDCartOrderStatus(int statusID, string statusText)
        {
            StatusID = statusID;
            StatusText = statusText;
        }

        /// <summary>
        /// The 3dcart order Status ID
        /// </summary>
        public int StatusID
        {
            get;
            set;
        }

        /// <summary>
        /// The 3dcart order Status Text
        /// </summary>
        public string StatusText
        {
            get;
            set;
        }
    }
}
