using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// The response from executing an HttpPostRequest
    /// </summary>
    public sealed class HttpResponseReader : IHttpResponseReader
    {
        private string result = null;
        private HttpWebRequest webRequest;
        private HttpWebResponse webResponse;

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpResponseReader(HttpWebRequest webRequest, HttpWebResponse webResponse, long responseTime)
        {
            MethodConditions.EnsureArgumentIsNotNull(webRequest, nameof(webRequest));
            MethodConditions.EnsureArgumentIsNotNull(webResponse, nameof(webResponse));

            this.webRequest = webRequest;
            this.webResponse = webResponse;
            ResponseTimeInMs = responseTime;
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
        /// Get the result string of the post
        /// </summary>
        public Task<string> ReadResultAsync() => ReadResultAsync(Encoding.UTF8);

        /// <summary>
        /// Get the result string of the post
        /// </summary>
        public async Task<string> ReadResultAsync(Encoding encoding)
        {
            if (result == null)
            {
                using (Stream stream = webResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, encoding, true))
                    {
                        result = await reader.ReadToEndAsync().ConfigureAwait(false);
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
        /// Time taken to get the response
        /// </summary>
        public long ResponseTimeInMs { get; set; }

        /// <summary>
        /// Dipose underlying objects
        /// </summary>
        public void Dispose()
        {
            webResponse.Close();
        }
    }
}
