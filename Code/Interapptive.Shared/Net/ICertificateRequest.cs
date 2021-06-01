using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// An interface for issuing requests with the intent of inspecting the certificate to 
    /// determine its security level (if any). Since the HttpRequest does not allow for mocking
    /// the ServicePoint.Certificate property, this interface was primarily created so we
    /// could effectively test the various certificate inspectors. 
    /// </summary>
    public interface ICertificateRequest
    {
        /// <summary>
        /// Gets the certificate provided by the host the request was submitted to.
        /// </summary>
        X509Certificate Certificate { get; }

        /// <summary>
        /// Gets the service point that the request was submitted to.
        /// </summary>
        ServicePoint ServicePoint { get; }

        /// <summary>
        /// Submits the request to the endpoint defined in the constructor and
        /// uses and determines the security level of the host.
        /// </summary>
        /// <returns>A CertificateSecurityLevel value indicating the security/trustworthiness.</returns>
        CertificateSecurityLevel Submit(bool force = false);

    }
}
