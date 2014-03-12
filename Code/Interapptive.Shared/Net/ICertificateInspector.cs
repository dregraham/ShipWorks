
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
        /// <param name="request">The request containing the certificate being inspected.</param>
        /// <returns>A CertificateSecurityLevel value indicating the trust/security level 
        /// of the certificate based on what the inspector is looking for.</returns>
        CertificateSecurityLevel Inspect(ICertificateRequest request);
    }
}
