using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// An implementation of the ICertificateInspector interface for determining whether the certificate
    /// associated with an ICertificateRequest is a valid FedEx certificate.
    /// </summary>
    public class CertificateInspector : ICertificateInspector
    {
        private readonly List<string> expectedCertificateSubjectElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateInspector"/> class.
        /// </summary>
        /// <param name="verificationData">The data that should be used to verify the certificate.</param>
        public CertificateInspector(string verificationData)
        {
            expectedCertificateSubjectElements = new List<string>();

            if (!string.IsNullOrWhiteSpace(verificationData))
            {
                // Use the credential store to load up the data elements we'll be looking for in
                // the subject of the certificate
                XDocument doc = XDocument.Parse(verificationData);
                foreach (XElement element in doc.Element("Subject").Elements("Value"))
                {
                    expectedCertificateSubjectElements.Add(element.Value.ToLower());
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
                    string subject = certificate.Subject.ToLower();
                    // There is a certificate, so we need to check the subject to make sure
                    // it contains all of the data elements we're expecting
                    if (!expectedCertificateSubjectElements.All(e => subject.Contains(e)))
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
