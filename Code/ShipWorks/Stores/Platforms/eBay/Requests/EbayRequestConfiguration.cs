using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// A class that holds configuration/metadata for an eBay request including 
    /// credentials, version of the API, request timeout, and a description of the 
    /// request being made.
    /// </summary>
    public class EbayRequestConfiguration
    {
        private string requestName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayRequestConfiguration"/> class.
        /// </summary>
        public EbayRequestConfiguration(string requestName)
        {
            this.requestName = requestName;
            this.Timeout = TimeSpan.FromMilliseconds(180 * 1000);
        }

        /// <summary>
        /// Gets the API version.
        /// </summary>
        public string ApiVersion
        {
            get { return "847"; }
        }
        
        /// <summary>
        /// Gets the exact name of the eBay web service request to be executed.
        /// </summary>
        public string RequestName
        {
            get { return requestName; }
        }

        /// <summary>
        /// Gets or sets the timeout for the call
        /// </summary>
        public TimeSpan Timeout
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url 
        { 
            get; 
            set; 
        }
    }
}
