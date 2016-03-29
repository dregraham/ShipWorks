using System;
using System.Text;

namespace Interapptive.Shared.Net
{
    public class HttpJsonVariableRequestSubmitter : HttpVariableRequestSubmitter
    {
        private const string ResponseFormat = "application/json";

        public string RequestBody { get; set; }

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
            // Not URL encoding the content since it is being posted as XML
            return Encoding.Default.GetBytes(RequestBody);
        }

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