
namespace Interapptive.Shared.Net
{
    /// <summary>
    /// An implementation of the ICertificateInspector interface that will trust any
    /// request that it inspects. This is somewhat analogous to the null object pattern
    /// and can be useful in cases where there are those situations where we are not
    /// concerned with trying to take additional measures to confirm that the certificate
    /// is genuine. For example, when sending a customer's shipping account credentials to a
    /// carrier's API, we would probably use this inspector, but if we are sending the 
    /// credentials that ShipWorks uses for counter rate accounts, we would probably want
    /// to use a different inspector.
    /// </summary>
    public class TrustingCertificateInspector : ICertificateInspector
    {
        /// <summary>
        /// Inspects the certificate of the given request to determine the security
        /// level of the certificate.
        /// </summary>
        /// <param name="request">The request containing the certificate being inspected.</param>
        /// <returns>Always returns a CertificateSecurityLevel of Trusted.</returns>
        public CertificateSecurityLevel Inspect(System.Net.HttpWebRequest request)
        {
            return CertificateSecurityLevel.Trusted;
        }
    }
}
