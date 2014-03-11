using System.Web;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// An interface intended to inspect the certificate associated with a request
    /// to determine the security level.
    /// </summary>
    public interface ICertificateInspector
    {
        /// <summary>
        /// Inspects the certificate of the given request to determine the security
        /// level of the certificate.
        /// </summary>
        CertificateSecurityLevel Inspect(HttpRequest request);
    }
}
