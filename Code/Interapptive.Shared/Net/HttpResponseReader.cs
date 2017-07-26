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
        string result = null;

        HttpWebRequest webRequest;
        HttpWebResponse webResponse;

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpResponseReader(HttpWebRequest webRequest, HttpWebResponse webResponse)
        {
            MethodConditions.EnsureArgumentIsNotNull(webRequest, nameof(webRequest));
            MethodConditions.EnsureArgumentIsNotNull(webResponse, nameof(webResponse));

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
        /// Dipose underlying objects
        /// </summary>
        public void Dispose()
        {
            webResponse.Close();
        }
    }
}
