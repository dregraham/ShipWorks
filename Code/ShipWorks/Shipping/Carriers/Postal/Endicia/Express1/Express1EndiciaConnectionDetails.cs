using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// The conenction details for the Express1 for Endicia integration
    /// </summary>
    public class Express1EndiciaConnectionDetails : IExpress1ConnectionDetails
    {
        /// <summary>
        /// Gets the franchise ID (company code)
        /// </summary>
        public string FranchiseId
        {
            get
            {
                return "306";
            }
        }

        /// <summary>
        /// Gets the API key for Express1 integrations (the "partner ID" in Endicia parlance)
        /// </summary>
        public string ApiKey
        {
            get 
            {
                if (TestServer)
                {
                    return "1A4E8B5F-99D4-422F-8782-7EBA12DDE312";
                }
                else
                {
                    return "2BFD18C9-EB52-4C64-867C-0E5328D264C8";
                }
            }
        }

        /// <summary>
        /// Gets the ID of the certificate to use to encrypt data
        /// </summary>
        public string CertificateId
        {
            get
            {
                if (TestServer)
                {
                    return "691F6E51-01E4-43BA-B5D4-643644C325DE";
                }
                else
                {
                    return "33DC2051-3645-4D65-B4DE-A5ECC3676EE1";
                }
            }
        }

        /// <summary>
        /// Determines if the Live Server should be used
        /// </summary>
        public bool TestServer
        {
            get { return Express1EndiciaUtility.UseTestServer; }
        }

        /// <summary>
        /// Gets the logging source for API calls
        /// </summary>
        public ApiLogSource ApiLogSource
        {
            get
            {
                return ApiLogSource.UspsExpress1Endicia;
            }
        }
    }
}
