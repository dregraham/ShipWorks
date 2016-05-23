using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Contains details used for the Express1 gateway
    /// </summary>
    public interface IExpress1ConnectionDetails
    {
        /// <summary>
        /// Gets the franchise Id (company code)
        /// </summary>
        string FranchiseId { get; }

        /// <summary>
        /// Gets the API key for Express1 integrations
        /// </summary>
        string ApiKey { get; }

        /// <summary>
        /// Gets the id of the certificate to use to encrypt data
        /// </summary>
        string CertificateId { get; }

        /// <summary>
        /// Determines if the Live Server should be used
        /// </summary>
        bool TestServer { get; }

        /// <summary>
        /// Gets the logging source for api calls
        /// </summary>
        ApiLogSource ApiLogSource { get; }
    }
}