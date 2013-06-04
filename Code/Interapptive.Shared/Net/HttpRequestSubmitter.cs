using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Base for various types of post requests
    /// </summary>
    public abstract class HttpRequestSubmitter
    {
        Uri uri;
        ICredentials credentials;

        // The verb being sent
        HttpVerb requestVerb = HttpVerb.Post;

        // Timeout
        TimeSpan timeout = TimeSpan.FromSeconds(90);

        // Send the Expect:100-continue header, default to .NET default
        bool expect100Continue = true;

        // Allow .NET to handle redirect errors by automatically re-requesting.  default to the .NET default of True
        bool allowAutoRedirect = true;

        // HTTP KeepAlive, defaulting to the .NET default
        bool keepAlive = true;

        /// <summary>
        /// The ContentType to specify in the request
        /// </summary>
        string contentType;

        // http headers to add to the request
        WebHeaderCollection headers = new WebHeaderCollection();

        // HttpStatusCodes which will be treated as "OK" when responses are received, no exception will be thrown
        List<HttpStatusCode> allowableStatusCodes = new List<HttpStatusCode>();

        /// <summary>
        /// The headers to add to the request
        /// </summary>
        public WebHeaderCollection Headers
        {
            get { return headers; }
        }

        /// <summary>
        /// Gets or sets whether or not HTTP Redirect/Moved codes will cause a re-request automatically
        /// </summary>
        public bool AllowAutoRedirect
        {
            get { return allowAutoRedirect; }
            set { allowAutoRedirect = value; }
        }

        /// <summary>
        /// Get or Set whether or not the Expect http header is sent with the request
        /// </summary>
        public bool Expect100Continue
        {
            get { return expect100Continue; }
            set { expect100Continue = value; }
        }

        /// <summary>
        /// HTTP KeepAlive
        /// </summary>
        public bool KeepAlive
        {
            get { return keepAlive; }
            set { keepAlive = value; }
        }

        /// <summary>
        /// Gets the ContentType to be specified on the request
        /// </summary>
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        /// <summary>
        /// Raised when the request is about to be submitted
        /// </summary>
        public event HttpRequestSubmittingEventHandler RequestSubmitting;

        /// <summary>
        /// Default constructor for an Http POST request.
        /// </summary>
        protected HttpRequestSubmitter()
            : this(HttpVerb.Post)
        {

        }

        /// <summary>
        /// Constructor for specifying the request verb
        /// </summary>
        protected HttpRequestSubmitter(HttpVerb verb)
        {
            this.requestVerb = verb;
        }

        /// <summary>
        /// These status codes will not cause an exception to be raised
        /// </summary>
        public void AllowHttpStatusCodes(params HttpStatusCode[] statuses)
        {
            allowableStatusCodes.AddRange(statuses);
        }

        /// <summary>
        /// Execute the request
        /// </summary>
        public virtual IHttpResponseReader GetResponse()
        {
            // Get the request Uri
            Uri requestUri = PrepareRequestUri();

            HttpWebRequest webRequest = null;
            try
            {
                webRequest = WebRequest.Create(requestUri) as HttpWebRequest;
                if (webRequest == null)
                {
                    throw new WebException("The URL is not a valid HTTP address.");
                }
            }
            catch (NotSupportedException)
            {
                throw new WebException("The URL is not a valid HTTP address.");
            }

            // Set credentials and timeout
            webRequest.Credentials = credentials;
            webRequest.Timeout = (int) Timeout.TotalMilliseconds;
            webRequest.ServicePoint.Expect100Continue = expect100Continue;
            webRequest.KeepAlive = keepAlive;

            if (headers.Count > 0)
            {
                foreach (string headerName in headers)
                {
                    webRequest.Headers.Add(headerName, headers[headerName]);
                }
            }

            // Configure request method
            SetRequestMethod(webRequest);

            // If it's not get set the content type and redirect option
            if (Verb != HttpVerb.Get)
            {
                webRequest.ContentType = contentType;
                webRequest.AllowAutoRedirect = allowAutoRedirect;
            }

            // Raise the posting event
            if (RequestSubmitting != null)
            {
                RequestSubmitting(this, new HttpRequestSubmittingEventArgs(webRequest));
            }
            
            // If not Get, then send the post data
            if (Verb != HttpVerb.Get)
            {
                // Setup the content
                byte[] content = GetPostContent();

                // Set the content data in the request
                webRequest.ContentLength = content.Length;

                // Submit the request to the server
                using (Stream postStream = webRequest.GetRequestStream())
                {
                    postStream.Write(content, 0, content.Length);
                }
            }

            // Get the response
            HttpWebResponse webResponse;

            try
            {
                webResponse = (HttpWebResponse) webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                // if we are allowing certain status codes to not throw exceptions, intercept here
                if (ex.Response != null &&
                    ex.Response is HttpWebResponse &&
                    allowableStatusCodes.Contains(((HttpWebResponse)ex.Response).StatusCode))
                {
                    webResponse = (HttpWebResponse)ex.Response;
                }
                else
                {
                    throw;
                }
            }

            // Check for OK
            if (webResponse.StatusCode != HttpStatusCode.OK && 
                !allowableStatusCodes.Contains(webResponse.StatusCode))
            {
                string statusDescription = webResponse.StatusDescription;
                webResponse.Close();

                throw new WebException(statusDescription);
            }

            return new HttpResponseReader(webRequest, webResponse);
        }

        /// <summary>
        /// Prepare the URI of the request
        /// </summary>
        private Uri PrepareRequestUri()
        {
            string query = GetQueryString();

            if (string.IsNullOrEmpty(query))
            {
                return Uri;
            }
            else
            {
                return new Uri(Uri.ToString() + ((Uri.Query.Length > 0) ? "&" : "?") + query);
            }
        }

        /// <summary>
        /// Configures the web request Method based on the Verb
        /// </summary>
        private void SetRequestMethod(HttpWebRequest webRequest)
        {
            switch (requestVerb)
            {
                case HttpVerb.Get:
                    webRequest.Method = "GET";
                    break;

                case HttpVerb.Post:
                    webRequest.Method = "POST";
                    break;

                case HttpVerb.Put:
                    webRequest.Method = "PUT";
                    break;

                default:
                    throw new InvalidOperationException("Invalid request verb: " + requestVerb);
            }
        }

        /// <summary>
        /// Gets the query string to use ro the request.  Applicable reqardless of GET or POST.
        /// </summary>
        protected virtual string GetQueryString()
        {
            return null;
        }

        /// <summary>
        /// Allows derived classes to prepare the content of the request.  Not called for GET requests.
        /// </summary>
        public virtual byte[] GetPostContent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The URI that will be posted to.
        /// </summary>
        [XmlIgnore]
        public Uri Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        /// <summary>
        /// Gets or sets authentication information for the request
        /// </summary>
        [XmlIgnore]
        public ICredentials Credentials
        {
            get { return credentials; }
            set { credentials = value; }
        }

        /// <summary>
        /// Gets or sets the timeout value.
        /// </summary>
        public TimeSpan Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        /// <summary>
        /// Gets or sets the request verb
        /// </summary>
        public HttpVerb Verb
        {
            get { return requestVerb; }
            set { requestVerb = value; }
        }
    }
}
