using System.Text;

namespace Interapptive.Shared.Net
{
    public class HttpXmlVariableRequestSubmitter : HttpVariableRequestSubmitter
    {
        private const string ResponseFormat = "application/xml";

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpXmlVariableRequestSubmitter"/> class.
        /// </summary>
        public HttpXmlVariableRequestSubmitter()
        {
            // Subscribe to the request submitting event, so we can set the content type
            // and what format we expect the response to be in
            RequestSubmitting += OnRequestSubmitting;

            ContentType = "application/xml";
        }

        /// <summary>
        /// Called when [request submitting]. We need to intercept the request, so the
        /// content type and response format can be set to XML.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Interapptive.Shared.Net.HttpRequestSubmittingEventArgs"/> instance containing the event data.</param>
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
            StringBuilder content = new StringBuilder();
            foreach(HttpVariable variable in Variables)
            {
                if (variable.Name.Length > 0)
                {
                    content.Append(variable.Name);
                    content.Append("=");
                }

                content.Append(variable.Value);
            }

            // Not URL encoding the content since it is being posted as XML
            return UTF8Encoding.Default.GetBytes(content.ToString());
        }
    }
}
