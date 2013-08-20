using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Actions.Tasks.Common.Enums;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.XPath;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task to hit a URL.
    /// </summary>
    [ActionTask("Send web request", "WebRequest")]
    public class WebRequestTask : TemplateBasedTask
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(WebRequestTask));

        private readonly ApiLogEntry requestLogger;

        public WebRequestTask()
        {
            requestLogger = new ApiLogEntry(ApiLogSource.WebRequestTask, "WebRequestTask");
        }

        /// <summary>
        /// Gets or sets the verb.
        /// </summary>
        public HttpVerb Verb { get; set; }

        /// <summary>
        /// Gets or sets the URL to hit.
        /// </summary>
        public string UrlToHit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use basic authentication].
        /// </summary>
        public bool UseBasicAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the HTTP headers.
        /// </summary>
        public KeyValuePair<string, string>[] HttpHeaders { get; set; }

        /// <summary>
        /// Gets a value indicating whether to postpone running or not.
        /// </summary>
        public override bool EnablePostpone
        {
            get { return false; }
        }

        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public override string InputLabel
        {
            get { return "Using"; }
        }

        /// <summary>
        /// Gets the input requirement for this task type (Optional).
        /// </summary>
        public override ActionTaskInputRequirement RequiresInput
        {
            get { return ActionTaskInputRequirement.Optional; }
        }

        /// <summary>
        /// Gets or sets the web request cardinality.
        /// </summary>
        public WebRequestCardinality RequestCardinality { get; set; }

        /// <summary>
        /// The object is being deserialized into its values
        /// </summary>
        public override void DeserializeXml(XPathNavigator xpath)
        {
            base.DeserializeXml(xpath);

            XPathNodeIterator headerIterator = xpath.Select("/Settings/HttpHeaders/*/@value");

            List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
            foreach (XPathNavigator header in headerIterator)
            {
                string[] keyValue = ((string)header.TypedValue).Split(',');

                // ignore the first character as it is "["
                string key = keyValue[0].Substring(1).Trim();

                // ignore the last character as it is "]"
                string value = keyValue[1].Substring(0, keyValue[1].Length - 1).Trim();

                headerList.Add(new KeyValuePair<string, string>(key, value));
            }

            HttpHeaders = headerList.ToArray();
        }

        /// <summary>
        /// Create the editor for the settings
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new WebRequestTaskEditor(this);
        }

        /// <summary>
        /// Sends template to URL (or just sends request if no Post Data)
        /// </summary>
        protected override void ProcessTemplateResults(TemplateEntity template, IList<TemplateResult> templateResults, ActionStepContext context)
        {
            foreach (TemplateResult templateResult in templateResults)
            {
                HttpTextPostRequestSubmitter request = new HttpTextPostRequestSubmitter(templateResult.ReadResult(), GetContentType((TemplateOutputFormat)template.OutputFormat));

                templateResult.XPathSource.Context.ProcessingComplete = false;
                string url = TemplateTokenProcessor.ProcessTokens(UrlToHit, templateResult.XPathSource);

                ProcessRequest(request, url);
            }
        }

        /// <summary>
        /// Run the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            switch (RequestCardinality)
            {
                case WebRequestCardinality.OneRequestPerFilterResult:
                    foreach (long inputKey in inputKeys)
                    {
                        string processedUrl = TemplateTokenProcessor.ProcessTokens(UrlToHit, inputKey);
                        ProcessRequest(request, processedUrl);
                    }

                    break;

                case WebRequestCardinality.SingleRequest:

                    string singleRequestProcessedUrl = TemplateTokenProcessor.ProcessTokens(UrlToHit, inputKeys);
                    ProcessRequest(request, singleRequestProcessedUrl);
                    
                    break;

                case WebRequestCardinality.OneRequestPerTemplateResult:
                    if (TemplateID == 0)
                    {
                        throw new ActionTaskRunException("No template selected.");
                    }
                    
                    base.Run(inputKeys, context);
                    
                    break;

                default:
                    throw new InvalidOperationException("Unknown Request Method Type in Run command.");
            }
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="request">The request being submitted.</param>
        /// <param name="url">The URL.</param>
        /// <exception cref="ActionTaskRunException">
        /// Url not formatted properly.
        /// or
        /// Error hitting URL.
        /// </exception>
        private void ProcessRequest(HttpRequestSubmitter request, string url)
        {
            try
            {
                request.Uri = new Uri(url);

                request.Verb = Verb;
                request.AllowAutoRedirect = true;

                AddRequestHeaders(request);

                // We want to allow all status codes so we can inspect them ourselves
                HttpStatusCode[] allStatusCodes = (HttpStatusCode[])Enum.GetValues(typeof(HttpStatusCode));
                HttpStatusCode[] allowedStatusCodes = allStatusCodes.Where(x => x != HttpStatusCode.Continue).ToArray();
                request.AllowHttpStatusCodes(allowedStatusCodes);

                // Submit the request, logging both the original request and the response
                LogFormattedRequest(request);
                IHttpResponseReader httpResponseReader = request.GetResponse();
                requestLogger.LogResponse(httpResponseReader.ReadResult(), "log");

                if ((int)httpResponseReader.HttpWebResponse.StatusCode >= 400)
                {
                    // A bad response was received that should cause the task to fail
                    log.ErrorFormat("An invalid response was received from the server when submitting the request to {0}: {1} {2}",
                                    request.Uri.AbsoluteUri, (int)httpResponseReader.HttpWebResponse.StatusCode, httpResponseReader.HttpWebResponse.StatusDescription);

                    throw new ActionTaskRunException(string.Format("An invalid response was received from {0}", request.Uri.AbsoluteUri));
                }
            }
            catch (UriFormatException ex)
            {
                log.Error("Error in WebRequest Url format.", ex);
                throw new ActionTaskRunException("Url not formatted properly.", ex);
            }
            catch (WebException ex)
            {
                // Leverage the crash submitter to extract and format the exception details, and remove 
                // any portion of the message that would show the callstack since the log will be in plain 
                // text to the user
                string exceptionDetail = CrashSubmitter.GetExceptionDetail(ex);
                string message = exceptionDetail.Substring(0, exceptionDetail.IndexOf("Callstack:", StringComparison.OrdinalIgnoreCase));

                requestLogger.LogResponse(message, "log");

                throw new ActionTaskRunException(string.Format("Web request returned the following error: {0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// Adds the authentication header (if needed) and all other headers to the request.
        /// </summary>
        /// <param name="request">The request.</param>
        private void AddRequestHeaders(HttpRequestSubmitter request)
        {
            if (UseBasicAuthentication)
            {
                // .NET typically waits for a server challenge before sending authorization, so force the authorization headers 
                //  be sent rather than using a NetworkCredential object
                string authInfo = string.Format("{0}:{1}", Username, Password);
                string encodedAuthInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                request.Headers.Add("Authorization", "Basic " + encodedAuthInfo);
            }

            foreach (KeyValuePair<string, string> header in HttpHeaders)
            {
                try
                {
                    request.Headers.Add(HttpUtility.UrlDecode(header.Key), HttpUtility.UrlDecode(header.Value));
                }
                catch (ArgumentException ex)
                {
                    log.Error("Error adding header in WebRequestTask", ex);
                    throw new ActionTaskRunException("Invalid header for WebRequest task.", ex);
                }
            }
        }

        /// <summary>
        /// Formats and logs the request details.
        /// </summary>
        /// <param name="request">The request.</param>
        private void LogFormattedRequest(HttpRequestSubmitter request)
        {
            StringBuilder headerText = new StringBuilder();

            foreach (string key in request.Headers.Keys)
            {
                string value = request.Headers[key];

                headerText.AppendLine(string.Format("{0}: {1}", key, value));
            }

            string postContent = Encoding.Default.GetString(request.GetPostContent());
            string message = string.Format("{0} {1} {2}{3}{2}{2}{4}", request.Verb.ToString().ToUpper(), request.Uri.AbsoluteUri, Environment.NewLine, headerText, postContent);

            requestLogger.LogRequest(message, "log");
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Invalid template output format</exception>
        private static string GetContentType(TemplateOutputFormat templateOutputFormat)
        {
            switch (templateOutputFormat)
            {
                case TemplateOutputFormat.Html:
                    return "text/html";
                case TemplateOutputFormat.Xml:
                    return "text/xml";
                case TemplateOutputFormat.Text:
                    return "";
                default:
                    throw new InvalidOperationException("Invalid template output format");
            }
        }
    }
}