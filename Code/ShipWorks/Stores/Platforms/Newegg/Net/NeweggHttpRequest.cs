using System;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.Newegg.Net
{
    /// <summary>
    /// An implementation of the INeweggRequest interface that submits requests via HTTP.
    /// </summary>
    public class NeweggHttpRequest : INeweggRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggHttpRequest"/> class.
        /// </summary>
        public NeweggHttpRequest()
        { }

        /// <summary>
        /// Submits the request with the given credentials and request configuration.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="requestConfiguration">The request configuration.</param>
        /// <returns>
        /// A NeweggResponse containing the response from the Newegg API.
        /// </returns>
        public async Task<string> SubmitRequest(Credentials credentials, RequestConfiguration requestConfiguration)
        {
            try
            {
                // Setup the request with the specifics for submitting the request to Newegg
                HttpXmlVariableRequestSubmitter submitter = CreateXmlHttpVariableRequestSubmitter(credentials, requestConfiguration);
                LogRequest(submitter, requestConfiguration);

                using (IHttpResponseReader reader = await submitter.GetResponseAsync().ConfigureAwait(false))
                {
                    string responseData = reader.ReadResult();
                    LogResponseText(responseData, requestConfiguration);

                    return responseData;
                }
            }
            catch (WebException ex)
            {
                throw new NeweggException("An error occurred contacting Newegg. Please try again later.", ex);
            }
        }

        /// <summary>
        /// A factory method for creating/configuring an XmlHttpVariableRequestSubmitter.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="requestConfiguration">The request configuration.</param>
        /// <returns>The XmlHttpVariableRequestSubmitter object.</returns>
        private static HttpXmlVariableRequestSubmitter CreateXmlHttpVariableRequestSubmitter(Credentials credentials, RequestConfiguration requestConfiguration)
        {
            HttpXmlVariableRequestSubmitter submitter = new HttpXmlVariableRequestSubmitter();
            submitter.Uri = new Uri(requestConfiguration.Url);
            submitter.Timeout = new TimeSpan(0, 0, requestConfiguration.TimeoutInSeconds);
            submitter.AllowAutoRedirect = false;

            submitter.Verb = requestConfiguration.Method;
            submitter.Variables.Add(string.Empty, requestConfiguration.Body);

            submitter.AllowHttpStatusCodes(new HttpStatusCode[] { HttpStatusCode.BadRequest, HttpStatusCode.Forbidden });

            submitter.Headers.Clear();
            submitter.Headers.Add("SecretKey", credentials.SecretKey);
            submitter.Headers.Add("Authorization", credentials.AuthorizationKey);

            return submitter;
        }

        /// <summary>
        /// Logs the request.
        /// </summary>
        /// <param name="submitter">The submitter.</param>
        /// <param name="requestConfiguration">The request configuration.</param>
        protected virtual void LogRequest(HttpXmlVariableRequestSubmitter submitter, RequestConfiguration requestConfiguration)
        {
            ApiLogEntry log = new ApiLogEntry(ApiLogSource.Newegg, requestConfiguration.Description);
            log.LogRequest(submitter);
        }

        /// <summary>
        /// Logs the response.
        /// </summary>
        /// <param name="xmlResponse">The XML response.</param>
        /// <param name="requestConfiguration">The request configuration.</param>
        protected virtual void LogResponseText(string xmlResponse, RequestConfiguration requestConfiguration)
        {
            ApiLogEntry log = new ApiLogEntry(ApiLogSource.Newegg, requestConfiguration.Description);
            log.LogResponse(xmlResponse);
        }
    }
}
