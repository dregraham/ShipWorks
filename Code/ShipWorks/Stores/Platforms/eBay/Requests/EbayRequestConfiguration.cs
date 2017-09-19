using System;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// A class that holds configuration/metadata for an eBay request including
    /// credentials, version of the API, request timeout, and a description of the
    /// request being made.
    /// </summary>
    public class EbayRequestConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EbayRequestConfiguration"/> class.
        /// </summary>
        public EbayRequestConfiguration(string requestName)
        {
            RequestName = requestName;
            Timeout = TimeSpan.FromMilliseconds(180 * 1000);
        }

        /// <summary>
        /// Gets the API version.
        /// </summary>
        public string ApiVersion => "1025";

        /// <summary>
        /// Gets the exact name of the eBay web service request to be executed.
        /// </summary>
        public string RequestName { get; }

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
