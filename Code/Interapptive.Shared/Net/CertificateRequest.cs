using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// A request that can be used for determining the security level of an endpoint based
    /// on the certificate of the host and on the rules of a given ICertificateInspector
    /// implementation.
    /// </summary>
    public class CertificateRequest : ICertificateRequest
    {
        private readonly HttpWebRequest webRequest;
        private readonly ICertificateInspector inspector;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateRequest"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="inspector">The inspector.</param>
        public CertificateRequest(Uri uri, ICertificateInspector inspector)
            : this(WebRequest.Create(uri) as HttpWebRequest, inspector)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateRequest"/> class.
        /// </summary>
        /// <param name="webRequest">The web request.</param>
        /// <param name="inspector">The inspector.</param>
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
        public CertificateSecurityLevel Submit()
        {
            try
            {
                using (WebResponse response = webRequest.GetResponse())
                {}
            }
            catch (WebException)
            {
                // Do nothing here, so a request to a vendor that results in an error code
                // (e.g. 404, 500, etc.) can still be inspected
            }

            return inspector.Inspect(this);
        }
    }
}
