using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Verifies the security level of an Endicia request
    /// </summary>
    public class Express1EndiciaCertificateInspector : ICertificateInspector
    {
        private readonly ICounterRatesCredentialStore credentialStore;

        /// <summary>
        /// Creates an instance of the Express1EndiciaCertificateInspector
        /// </summary>
        public Express1EndiciaCertificateInspector() : 
            this(TangoCounterRatesCredentialStore.Instance)
        {
            
        }

        /// <summary>
        /// Creates an instance of the Express1EndiciaCertificateInspector
        /// </summary>
        /// <param name="credentialStore">Credential store that should be used</param>
        public Express1EndiciaCertificateInspector(ICounterRatesCredentialStore credentialStore)
        {
            this.credentialStore = credentialStore;
        }

        /// <summary>
        /// Inspects the certificate of the given request to determine the security
        /// level of the certificate.
        /// </summary>
        /// <param name="request">The request containing the certificate being inspected.</param>
        /// <returns>A CertificateSecurityLevel value indicating the trust/security level 
        /// of the certificate based on what the inspector is looking for.</returns>
        public CertificateSecurityLevel Inspect(HttpWebRequest request)
        {
            if (request.ServicePoint == null)
            {
                return CertificateSecurityLevel.None;
            }

            X509Certificate certificate = request.ServicePoint.Certificate;
            
            if (certificate == null)
            {
                return CertificateSecurityLevel.None;
            }

            XElement certificateChecks = XElement.Parse(credentialStore.Express1EndiciaCertificateVerificationData);

            return certificateChecks.Descendants("Value").Any(x => certificate.Subject.IndexOf(x.Value) == -1) ? 
                CertificateSecurityLevel.Spoofed : 
                CertificateSecurityLevel.Trusted;
        }
    }
}
