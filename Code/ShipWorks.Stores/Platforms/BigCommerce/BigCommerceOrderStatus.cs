using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Helper class that represents an online BigCommerce order status
    /// </summary>
    public class BigCommerceOrderStatus
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statusID">The BigCommerce order Status ID</param>
        /// <param name="statusText">The BigCommerce order Status Text</param>
        public BigCommerceOrderStatus(int statusID, string statusText)
        {
            StatusID = statusID;
            StatusText = statusText;
        }

        /// <summary>
        /// The BigCommerce order Status ID
        /// </summary>
        public int StatusID
        {
            get;
            set;
        }

        /// <summary>
        /// The BigCommerce order Status Text
        /// </summary>
        public string StatusText
        {
            get;
            set;
        }
    }
}
