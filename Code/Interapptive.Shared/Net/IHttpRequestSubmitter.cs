using System;
using System.Net;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Interface that represents a base for various types of post requests
    /// </summary>
    public interface IHttpRequestSubmitter
    {
        /// <summary>
        /// Gets or sets whether or not HTTP Redirect/Moved codes will cause a re-request automatically
        /// </summary>
        bool AllowAutoRedirect { get; set; }

        /// <summary>
        /// Gets or sets the ContentType to be specified on the request
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Gets or sets authentication information for the request
        /// </summary>
        ICredentials Credentials { get; set; }

        /// <summary>
        /// Get or Set whether or not the Expect http header is sent with the request
        /// </summary>
        bool Expect100Continue { get; set; }

        /// <summary>
        /// The headers to add to the request
        /// </summary>
        WebHeaderCollection Headers { get; }

        /// <summary>
        /// HTTP KeepAlive
        /// </summary>
        bool KeepAlive { get; set; }

        /// <summary>
        /// Gets or sets the timeout value.
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// The URI that will be posted to.
        /// </summary>
        Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the request verb
        /// </summary>
        HttpVerb Verb { get; set; }

        /// <summary>
        /// Raised when the request is about to be submitted
        /// </summary>
        event HttpRequestSubmittingEventHandler RequestSubmitting;

        /// <summary>
        /// These status codes will not cause an exception to be raised
        /// </summary>
        void AllowHttpStatusCodes(params HttpStatusCode[] statuses);

        /// <summary>
        /// Allows derived classes to prepare the content of the request.  Not called for GET requests.
        /// </summary>
        byte[] GetPostContent();

        /// <summary>
        /// Execute the request
        /// </summary>
        IHttpResponseReader GetResponse();


        /// <summary>
        /// Prepare the URI of the request
        /// </summary>
        Uri GetPreparedRequestUri();
    }
}