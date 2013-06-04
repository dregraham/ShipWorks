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
    }
}
