using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    /// <summary>
    /// Connection details for Express1-USPS integration
    /// </summary>
    public class Express1UspsConnectionDetails : IExpress1ConnectionDetails
    {
        /// <summary>
        /// Gets the franchise Id (company code)
        /// </summary>
        public string FranchiseId
        {
            get
            {
                return "297";
            }
        }

        /// <summary>
        /// Gets the API key for Express1 integrations
        /// </summary>
        public string ApiKey {
            get
            {
                return (TestServer) ? "E29E5BD9-2B23-4A86-A1C8-618799B52971" : "5DC584E8-6E59-4B7C-AE22-6E29E43B88C7";
            }
        }

        /// <summary>
        /// Gets the id of the certificate to use to encrypt data
        /// </summary>
        public string CertificateId
        {
            get
            {
                return (TestServer) ? "7F44DDDC-F708-4F41-B415-6FD568AEE26D" : "D5467943-8D18-47B3-B095-3DA091A664F0";
            }
        }

        /// <summary>
        /// Gets the url of the postage service to use for Stamps version of Express1
        /// </summary>
        public string ServiceUrl
        {
            get
            {
                return (TestServer) ?
                    "http://www.express1dev.com/Services/SDCV29Service.svc" :
                    "https://service.express1.com/Services/SDCV29Service.svc";
            }
        }

        /// <summary>
        /// Determines if the Live Server should be used
        /// </summary>
        public bool TestServer
        {
            get
            {
                return UseTestServer;
            }
        }

        /// <summary>
        /// Gets the logging source for api calls
        /// </summary>
        public ApiLogSource ApiLogSource
        {
            get
            {
                return ApiLogSource.UspsExpress1Stamps;
            }
        }

        /// <summary>
        /// Determines if the Live Server should be used
        /// </summary>
        public static bool UseTestServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("Express1StampsTestServer", false);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("Express1StampsTestServer", value);
            }
        }
    }
}
