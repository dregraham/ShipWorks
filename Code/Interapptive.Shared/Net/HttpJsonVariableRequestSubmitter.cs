using System;
using System.Text;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Http request submitter with the ability to make POST requests with a JSON body
    /// </summary>
    public class HttpJsonVariableRequestSubmitter : HttpVariableRequestSubmitter
    {
        private const string ResponseFormat = "application/json";

        /// <summary>
        /// Gets or sets the request body.
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpJsonVariableRequestSubmitter"/> class.
        /// </summary>
        public HttpJsonVariableRequestSubmitter()
        {
            // Subscribe to the request submitting event, so we can set the content type
            // and what format we expect the response to be in
            RequestSubmitting += OnRequestSubmitting;

            ContentType = ResponseFormat;
        }

        /// <summary>
        /// Called when [request submitting]. We need to intercept the request, so the
        /// content type and response format can be set to XML.
        /// </summary>
        void OnRequestSubmitting(object sender, HttpRequestSubmittingEventArgs e)
        {
            e.HttpWebRequest.Accept = ResponseFormat;
        }

        /// <summary>
        /// Prepare the content of the request. This is overridden since we don't want to
        /// URL encode the content
        /// </summary>
        public override byte[] GetPostContent()
        {
            // Not URL encoding the content since it is being posted as JSON
            return Encoding.Default.GetBytes(RequestBody);
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        public string ProcessRequest(IApiLogEntry logEntry, Type exceptionTypeToRethrow)
        {
            try
            {
                logEntry.LogRequest(this);

                using (IHttpResponseReader reader = GetResponse())
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "txt");

                    // Clear variables for next reqeust
                    Variables.Clear();

                    return responseData;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, exceptionTypeToRethrow);
            }
        }
    }
}