using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// The response from executing an HttpPostRequest
    /// </summary>
    public sealed class HttpResponseReader : IHttpResponseReader
    {
        string result = null;

        HttpWebRequest webRequest;
        HttpWebResponse webResponse;

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpResponseReader(HttpWebRequest webRequest, HttpWebResponse webResponse)
        {
            if (webRequest == null)
            {
                throw new ArgumentNullException("webRequest");
            }
            
            if (webResponse == null)
            {
                throw new ArgumentNullException("webResponse");
            }

            this.webRequest = webRequest;
            this.webResponse = webResponse;
        }

        /// <summary>
        /// Get the result string of the post
        /// </summary>
        public string ReadResult()
        {
            return ReadResult(Encoding.UTF8);
        }

        /// <summary>
        /// Get the result string of the post
        /// </summary>
        public string ReadResult(Encoding encoding)
        {
            if (result == null)
            {
                using (Stream stream = webResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, encoding, true))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// The original web request that was submitted
        /// </summary>
        public HttpWebRequest HttpWebRequest
        {
            get { return webRequest; }
        }

        /// <summary>
        /// The underlying web response that was received
        /// </summary>
        public HttpWebResponse HttpWebResponse
        {
            get { return webResponse; }
        }

        /// <summary>
        /// Dipose underlying objects
        /// </summary>
        public void Dispose()
        {
            webResponse.Close();
        }
    }
}
