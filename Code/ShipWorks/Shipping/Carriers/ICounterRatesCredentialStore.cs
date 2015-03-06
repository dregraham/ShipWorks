
namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// An interface for the credentials needed for accessing counter rates that allows 
    /// for the abstracting the underlying storage container.
    /// </summary>
    public interface ICounterRatesCredentialStore
    {
        /// <summary>
        /// Gets the FedEx account number used for obtaining counter rates.
        /// </summary>
        string FedExAccountNumber { get; }

        /// <summary>
        /// Gets the FedEx meter number used for obtaining counter rates.
        /// </summary>
        string FedExMeterNumber { get;  }

        /// <summary>
        /// Gets the FedEx username used for obtaining counter rates.
        /// </summary>
        string FedExUsername { get; }

        /// <summary>
        /// Gets the FedEx password used for obtaining counter rates.
        /// </summary>
        string FedExPassword { get; }

        /// <summary>
        /// Gets data to verify the SSL certificate from FedEx
        /// </summary>
        string FedExCertificateVerificationData { get; }

        /// <summary>
        /// Gets the Ups user id used for obtaining counter rates
        /// </summary>
        string UpsUserId { get; }

        /// <summary>
        /// Gets the Ups password used for obtaining counter rates
        /// </summary>
        string UpsPassword { get; }

        /// <summary>
        /// Gets the Ups access key used for obtaining counter rates
        /// </summary>
        string UpsAccessKey { get; }

        /// <summary>
        /// Gets data to verify the SSL certificate from Ups
        /// </summary>
        string UpsCertificateVerificationData { get; }

        /// <summary>
        /// Gets the USPS user name used for obtaining counter rates
        /// </summary>
        string UspsUsername { get; }

        /// <summary>
        /// Gets the USPS password used for obtaining counter rates
        /// </summary>
        string UspsPassword { get; }

        /// <summary>
        /// Gets data to verify the SSL certificate from USPS
        /// </summary>
        string UspsCertificateVerificationData { get; }
    }
}
