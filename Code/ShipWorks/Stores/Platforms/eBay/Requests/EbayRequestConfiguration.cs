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
        private string description;
        private string sandboxCredentialSaltValue = "apptive";

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayRequestConfiguration"/> class.
        /// </summary>
        public EbayRequestConfiguration(string description)
        {
            this.description = description;
            this.TimeoutInMilliseconds = 180 * 1000;
        }

        /// <summary>
        /// Gets the API version.
        /// </summary>
        public string ApiVersion
        {
            get
            {
                // Note: the GetUser call returns a warning with version 787 when a <DetailLevel> node 
                // is provided in the request. Be sure to check that this warning does not turn into 
                // an error when upgrading to future versions of the API.
                return "787";
            }
        }
        
        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets or sets the timeout in milliseconds.
        /// </summary>
        /// <value>
        /// The timeout in milliseconds.
        /// </value>
        public int TimeoutInMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }
               

        /// <summary>
        /// Gets the sandbox developer credential.
        /// </summary>
        public string SandboxDeveloperCredential
        {
            get { return SecureText.Decrypt("9EjIiY68ZC9AmdbCllBY3u7iAPOS7JnSlecbcc8Jf80=", sandboxCredentialSaltValue); }
        }

        /// <summary>
        /// Gets the sandbox application credential.
        /// </summary>
        public string SandboxApplicationCredential
        {
            get { return SecureText.Decrypt("d0t5tDPECtuYnbRi9LAXAsLrbVXGyhsVAW3Jo3giRNY=", sandboxCredentialSaltValue); }
        }

        /// <summary>
        /// Gets the sandbox certificate credential.
        /// </summary>
        public string SandboxCertificateCredential
        {
            get { return SecureText.Decrypt("Z9/F10grv8Vkkz/UU1Zk4I26AJ6ZBA2EdcCSgQMbUvM=", sandboxCredentialSaltValue); }
        }
    }
}
