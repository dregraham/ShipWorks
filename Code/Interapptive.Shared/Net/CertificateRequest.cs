using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using log4net;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// A request that can be used for determining the security level of an endpoint based
    /// on the certificate of the host and on the rules of a given ICertificateInspector
    /// implementation.
    /// </summary>
    public class CertificateRequest : ICertificateRequest
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(CertificateRequest));
        private readonly HttpWebRequest webRequest;
        private readonly ICertificateInspector inspector;
        private const int throttlePeriod = 15;
        private static DateTime nextSecureConnectionValidation = DateTime.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateRequest"/> class.
        /// </summary>
        public CertificateRequest(Uri uri, ICertificateInspector inspector)
            : this(WebRequest.Create(uri) as HttpWebRequest, inspector)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateRequest"/> class.
        /// </summary>
        public CertificateRequest(HttpWebRequest webRequest, ICertificateInspector inspector)
        {
            this.webRequest = webRequest;
            this.inspector = inspector;
        }

        /// <summary>
        /// Gets the certificate provided by the host the request was submitted to.
        /// </summary>
        public X509Certificate Certificate
        {
            get { return webRequest.ServicePoint.Certificate; }
        }

        /// <summary>
        /// Gets the service point that the request was submitted to.
        /// </summary>
        public ServicePoint ServicePoint
        {
            get { return webRequest.ServicePoint; }
        }

        /// <summary>
        /// Submits the request to the endpoint defined in the constructor and 
        /// uses the inspector to determine the security level of the host.
        /// </summary>
        public CertificateSecurityLevel Submit(bool force = false)
        {
            CertificateSecurityLevel level = CertificateSecurityLevel.Trusted;

            if (nextSecureConnectionValidation >= DateTime.UtcNow && !force)
            {
                return level;
            }

            log.Info("CertificateRequest.Submit: Checking the certificate.");

            try
            {
                using (WebResponse response = webRequest.GetResponse())
                { }
            }
            catch (WebException)
            {
                // Do nothing here, so a request to a vendor that results in an error code
                // (e.g. 404, 500, etc.) can still be inspected
            }

            level = inspector.Inspect(this);
            if (level == CertificateSecurityLevel.Trusted)
            {
                nextSecureConnectionValidation = DateTime.UtcNow.AddMinutes(throttlePeriod);
            } 
            else
            {
                log.Warn($"Certificate not trusted for '{webRequest?.RequestUri?.Host}'");
            }

            return level;
        }
    }
}
