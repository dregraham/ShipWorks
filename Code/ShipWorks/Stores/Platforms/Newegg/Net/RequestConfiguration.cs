using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.Newegg.Net
{
    /// <summary>
    /// Encapsulates the configuration data required to make a request to Newegg 
    /// via the INeweggRequest interface. 
    /// </summary>
    public class RequestConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestConfiguration"/> class.
        /// </summary>
        /// <param name="description">The description of what the request configuration is 
        /// expected to do (e.g. download orders, upload shipping details, etc.).</param>
        public RequestConfiguration(string description)
        {
            this.TimeoutInSeconds = 30;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the description of what the request configuration is expected
        /// to do (e.g. download orders, upload shipping details, etc.)
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the request method (GET, POST, PUT, etc.).
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public HttpVerb Method { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the timeout in seconds.
        /// </summary>
        /// <value>
        /// The timeout in seconds.
        /// </value>
        public int TimeoutInSeconds { get; set; }

    }
}
