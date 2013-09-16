using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Connection details for Express1 stamps integration
    /// </summary>
    public class Express1StampsConnectionDetails : IExpress1ConnectionDetails
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
                return "5DC584E8-6E59-4B7C-AE22-6E29E43B88C7";
            }
        }

        /// <summary>
        /// Gets the id of the certificate to use to encrypt data
        /// </summary>
        public string CertificateId
        {
            get
            {
                return "D5467943-8D18-47B3-B095-3DA091A664F0";
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
