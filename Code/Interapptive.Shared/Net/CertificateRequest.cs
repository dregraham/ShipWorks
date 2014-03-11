using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// A request that can be used for determining the security level of an endpoint based
    /// on the certificate of the host based on the rules of a given ICertificateInspector
    /// implementation.
    /// </summary>
    public class CertificateRequest
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
        /// Submits the request to the endpoint defined in the constructor and 
        /// uses the inspector to determine the security level of the host.
        /// </summary>
        public CertificateSecurityLevel Submit()
        {
            using (WebResponse response = webRequest.GetResponse())
            {
                return inspector.Inspect(webRequest);
            }
        }
    }
}
