using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Net
{

    /// <summary>
    /// Creates requested HttpRequestSubmitterFactory
    /// </summary>
    public class HttpRequestSubmitterFactory : IHttpRequestSubmitterFactory
    {
        /// <summary>
        /// Get an HttpBinaryPostRequestSubmitter
        /// </summary>
        public HttpRequestSubmitter GetHttpBinaryPostRequestSubmitter(byte[] postData)
        {
            return new HttpBinaryPostRequestSubmitter(postData);
        }

        /// <summary>
        /// Gets an HttpTextPostRequestSubmitter
        /// </summary>
        public HttpRequestSubmitter GetHttpTextPostRequestSubmitter(string text, string contentType)
        {
            return new HttpTextPostRequestSubmitter(text, contentType);
        }
    }
}
