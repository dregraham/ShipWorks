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
    public class HttpBinaryPostRequestSubmitter : HttpRequestSubmitter
    {
        byte[] postData;

        /// <summary>
        /// For serialization
        /// </summary>
        protected HttpBinaryPostRequestSubmitter()
        {

        }

        /// <summary>
        /// Create a new request that will post the given data
        /// </summary>
        public HttpBinaryPostRequestSubmitter(byte[] postData)
            : this(postData, "application/x-www-form-urlencoded")
        {

        }

        /// <summary>
        /// Create a new request that will post the given data
        /// </summary>
        public HttpBinaryPostRequestSubmitter(byte[] postData, string contentType)
        {
            this.postData = postData;

            // Set the content type
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
