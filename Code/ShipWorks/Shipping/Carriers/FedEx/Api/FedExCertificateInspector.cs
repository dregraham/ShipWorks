using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// An implementation of the ICertificateInspector interface for determining whether the certificate
    /// associated with an ICertificateRequest is a valid FedEx certificate.
    /// </summary>
    public class FedExCertificateInspector : ICertificateInspector
    {
        private readonly List<string> expectedCertificateSubjectElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCertificateInspector"/> class.
        /// </summary>
        public FedExCertificateInspector()
            : this(TangoCounterRatesCredentialStore.Instance)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCertificateInspector"/> class.
        /// </summary>
        /// <param name="credentialStore">The credential store.</param>
        public FedExCertificateInspector(ICounterRatesCredentialStore credentialStore)
        {
            expectedCertificateSubjectElements = new List<string>();

            if (!string.IsNullOrWhiteSpace(credentialStore.FedExCertificateVerificationData))
            {
                // Use the credential store to load up the data elements we'll be looking for in
                // the subject of the certificate
                XDocument doc = XDocument.Parse(credentialStore.FedExCertificateVerificationData);
                foreach (XElement element in doc.Element("Subject").Elements("Value"))
                {
                    expectedCertificateSubjectElements.Add(element.Value);
                }
            }
        }

        /// <summary>
        /// Inspects the certificate looking for FedEx data in the given request to 
        /// determine the security level of the certificate. 
        /// </summary>
        /// <param name="request">The request containing the certificate being inspected.</param>
        /// <returns> A CertificateSecurityLevel value indicating the trust/security level
        /// of the certificate based on what the inspector is looking for.</returns>
        public CertificateSecurityLevel Inspect(ICertificateRequest request)
        {
            // Optimistically set the security level to trusted in the event that there
            // are not any elements to check in the certificate
            CertificateSecurityLevel securityLevel = CertificateSecurityLevel.Trusted;

            if (expectedCertificateSubjectElements.Any())
            {
                // The credential store indicates that we need to inspect the certificate
                X509Certificate certificate = request.Certificate;
                if (certificate == null)
                {
                    securityLevel = CertificateSecurityLevel.None;
                }
                else
                {
                    // There is a certificate, so we need to check the subject to make sure
                    // it contains all of the data elements we're expecting
                    if (!expectedCertificateSubjectElements.All(e => certificate.Subject.Contains(e)))
                    {
                        // The certificate did not match all the elements - consider it as spoofed
                        securityLevel = CertificateSecurityLevel.Spoofed;
                    }
                }
            }

            return securityLevel;
        }
    }
}
