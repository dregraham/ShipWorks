﻿using System.Collections.Generic;
using System.Globalization;
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
                foreach (XElement element in doc.Element("Service").Element("Subject").Elements("Value"))
                {
                    expectedCertificateSubjectElements.Add(element.Value.ToUpper(CultureInfo.InvariantCulture));
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
            // Pessimistically set the security level to spoofed in the event that there
            // are not any elements to check in the certificate
            CertificateSecurityLevel securityLevel = CertificateSecurityLevel.Spoofed;

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
                    string subject = certificate.Subject.ToUpper(CultureInfo.InvariantCulture);
                    if (expectedCertificateSubjectElements.All(subject.Contains))
                    {
                        // The certificate matched all the elements - consider it trusted
                        securityLevel = CertificateSecurityLevel.Trusted;
                    }
                    else
                    {
                        securityLevel = CertificateSecurityLevel.Spoofed;
                    }
                }
            }

            return securityLevel;
        }
    }
}
