using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// A POST request for sending a single unnamed binary parameter
    /// </summary>
    public class HttpTextPostRequestSubmitter : HttpRequestSubmitter
    {
        byte[] postData;

        /// <summary>
        /// For serialization
        /// </summary>
        protected HttpTextPostRequestSubmitter()
        {

        }

        /// <summary>
        /// Create a new request that will post the given data
        /// </summary>
        public HttpTextPostRequestSubmitter(string text, string contentType)
            : this(text, contentType, Encoding.UTF8)
        {

        }

        /// <summary>
        /// Create a new request that will post the given data
        /// </summary>
        public HttpTextPostRequestSubmitter(string text, string contentType, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            this.postData = encoding.GetBytes(text);
            this.ContentType = contentType;
        }

        /// <summary>
        /// Generate the content for the reguest
        /// </summary>
        public override byte[] GetPostContent()
        {
            return postData;
        }
    }
}