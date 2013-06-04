using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Signature for the HttpRequestSubmitting
    /// </summary>
    public delegate void HttpRequestSubmittingEventHandler(object sender, HttpRequestSubmittingEventArgs e);

    /// <summary>
    /// Event args for the HttpRequestSubmitting
    /// </summary>
    public class HttpRequestSubmittingEventArgs : EventArgs
    {
        HttpWebRequest webRequest;

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpRequestSubmittingEventArgs(HttpWebRequest webRequest)
        {
            this.webRequest = webRequest;
        }

        /// <summary>
        /// The actual web request that is about to be posted
        /// </summary>
        public HttpWebRequest HttpWebRequest
        {
            get { return webRequest; }
        }
    }
}
