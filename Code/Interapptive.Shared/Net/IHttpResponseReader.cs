using System;
using System.Net;
using System.Text;

namespace Interapptive.Shared.Net
{
    public interface IHttpResponseReader : IDisposable
    {
        /// <summary>
        /// Get the result string of the post
        /// </summary>
        string ReadResult();

        /// <summary>
        /// Get the result string of the post
        /// </summary>
        string ReadResult(Encoding encoding);

        /// <summary>
        /// The original web request that was submitted
        /// </summary>
        HttpWebRequest HttpWebRequest
        {
            get;
        }

        /// <summary>
        /// The underlying web response that was received
        /// </summary>
        HttpWebResponse HttpWebResponse
        {
            get;
        }
    }
}