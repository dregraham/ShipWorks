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
using Interapptive.Shared.Utility;
using System.Text.RegularExpressions;
using Interapptive.Shared.Security;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task to submit a request to a specified URL.
    /// </summary>
    [ActionTask("Send web request", "WebRequest", ActionTaskCategory.External)]
    public class WebRequestTask : TemplateBasedTask
    {
        private const string PasswordSalt = "WebRequestTask";

        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(WebRequestTask));
        private readonly ApiLogEntry requestLogger;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestTask"/> class.
        /// </summary>
        public WebRequestTask()
        {
            requestLogger = new ApiLogEntry(ApiLogSource.WebRequestTask, "WebRequestTask");

            Username = string.Empty;
            EncryptedPassword = string.Empty;
        }

        /// <summary>
        /// Gets or sets the verb.
        /// </summary>
        public HttpVerb Verb 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the URL the request is being submitted to.
        /// </summary>
        public string Url 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use basic authentication].
        /// </summary>
        public bool UseBasicAuthentication 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string Username 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets the password in encrypted format. The getter is public so the value gets serialized, but the
        /// setter is private, so the deserialization process can set the raw, encrypted value and so consumers
        /// cannot directly encrypt the password themselves. 
        /// 
        /// It would probably be worth looking into having a separate Password class to manage this at
        /// some point to avoid having to maintain the separate states...
        /// </summary>
        public string EncryptedPassword 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Indicates if the HTTP headers that have been configured should be included in the request
        /// </summary>
        public bool IncludeCustomHttpHeaders 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the HTTP headers.
        /// </summary>
        public KeyValuePair<string, string>[] HttpHeaders 
        { 
            get; 
            set; 
        }

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
            get { return "Send the request using:"; }
        }

        /// <summary>
        /// Gets or sets the web request cardinality.
        /// </summary>
        public WebRequestCardinality RequestCardinality 
        { 
            get; 
            set; 
        }
        
        /// <summary>
        /// Uses the plain text password to encrypt and set the value backing the 
        /// Password property. 
        /// </summary>
        public void SetPassword(string plainTextPassword)
        {
            // This was added, so the various consumers (e.g. the web request editor, 
            // serialization/deserialization, etc.) of this operations on this task
            // don't have to try to maintain whether the password is encrypted or not.            
            EncryptedPassword = SecureText.Encrypt(plainTextPassword, PasswordSalt);
        }

        /// <summary>
        /// Gets the decrypted password. The separate, private Password property is only here
        /// so the task settings get serialized.
        /// </summary>
        public string GetDecryptedPassword()
        {
            return SecureText.Decrypt(EncryptedPassword, PasswordSalt);
        }

        /// <summary>
        /// Gets the input requirement for this task type (Optional).
        /// </summary>
        public override ActionTaskInputRequirement InputRequirement
        {
            get { return ActionTaskInputRequirement.Optional; }
        }

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
                string pairString = (string) header.TypedValue;

                Match match = Regex.Match(pairString, @"^\[(?<key>[^,]+?)\s*,\s*(?<value>.*)\]\s*$");
                if (match.Success)
                {
                    headerList.Add(new KeyValuePair<string, string>(match.Groups["key"].Value, match.Groups["value"].Value));
                }
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
                HttpTextPostRequestSubmitter request = new HttpTextPostRequestSubmitter(
                    templateResult.ReadResult(), 
                    GetContentType((TemplateOutputFormat) template.OutputFormat));

                templateResult.XPathSource.Context.ProcessingComplete = false;
                string url = TemplateTokenProcessor.ProcessTokens(Url, templateResult.XPathSource);

                ProcessRequest(
                    request,
                    url,
                    token =>
                    {
                        templateResult.XPathSource.Context.ProcessingComplete = false;
                        return TemplateTokenProcessor.ProcessTokens(token, templateResult.XPathSource);
                    });
            }
        }

        /// <summary>
        /// Run the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            var inputSource = (ActionTaskInputSource)context.Step.InputSource;

            // How we process individual keys depends on the cardinality
            switch (RequestCardinality)
            {
                // Single request to process - use all the input keys at once
                case WebRequestCardinality.SingleRequest:
                {
                    string processedUrl = (inputSource == ActionTaskInputSource.Nothing) ? Url : TemplateTokenProcessor.ProcessTokens(Url, inputKeys);

                    ProcessRequest(new HttpVariableRequestSubmitter(), processedUrl, token => TemplateTokenProcessor.ProcessTokens(token, inputKeys));
                    return;
                }

                // One request per filter result - in this case, we process per input key
                case WebRequestCardinality.OneRequestPerFilterResult:
                {
                    if (inputSource != ActionTaskInputSource.FilterContents)
                    {
                        throw new ActionTaskRunException("The task input is not a filter.");
                    }

                    // Go through and process for each key
                    foreach (long inputKey in inputKeys)
                    {
                        string processedUrl = TemplateTokenProcessor.ProcessTokens(Url, inputKey);

                        ProcessRequest(new HttpVariableRequestSubmitter(), processedUrl, token => TemplateTokenProcessor.ProcessTokens(token, inputKey));
                    }
                    return;
                }

                // Once per template result
                case WebRequestCardinality.OneRequestPerTemplateResult:
                {
                    if (TemplateID == 0)
                    {
                        throw new ActionTaskRunException("No template is selected.");
                    }

                    if (inputSource == ActionTaskInputSource.Nothing)
                    {
                        throw new ActionTaskRunException("The task has no input to use for the template.");
                    }

                    // Run it all through the base, which will take care of template processing
                    base.Run(inputKeys, context);
                    return;
                }
            }

            throw new ActionTaskRunException("The request configuration is invalid.");
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
        private void ProcessRequest(HttpRequestSubmitter request, string url, Func<string, string> tokenProcessor)
        {
            try
            {
                // Setup the request
                request.Uri = new Uri(url);
                request.Verb = Verb;
                request.AllowAutoRedirect = true;

                // Add the headers
                AddRequestHeaders(request, tokenProcessor);

                // We want to allow all status codes so we can inspect them ourselves
                HttpStatusCode[] allStatusCodes = (HttpStatusCode[])Enum.GetValues(typeof(HttpStatusCode));
                HttpStatusCode[] allowedStatusCodes = allStatusCodes.Where(x => x != HttpStatusCode.Continue).ToArray();
                request.AllowHttpStatusCodes(allowedStatusCodes);

                // Submit the request, logging both the original request and the response
                LogFormattedRequest(request);

                // Execute
                using (IHttpResponseReader httpResponseReader = request.GetResponse())
                {
                    // Log the full response, but ignore it
                    requestLogger.LogResponse(httpResponseReader.ReadResult(), "log");

                    int statusCode = (int) httpResponseReader.HttpWebResponse.StatusCode;
                    if (statusCode >= 400)
                    {
                        // A bad response was received that should cause the task to fail
                        log.ErrorFormat("An invalid response was received from the server when submitting the request to {0}: {1} {2}",
                            request.Uri.AbsoluteUri, 
                            statusCode, 
                            httpResponseReader.HttpWebResponse.StatusDescription);

                        throw new ActionTaskRunException(string.Format("A {0} response was received from {1}.", statusCode, request.Uri.AbsoluteUri));
                    }
                }
            }
            catch (UriFormatException ex)
            {
                log.Error("Error in WebRequest Url format.", ex);
                throw new ActionTaskRunException(string.Format("The URL '{0}' is not a valid.", url), ex);
            }
            catch (WebException ex)
            {
                // Leverage the crash submitter to extract and format the exception details, and remove 
                // any portion of the message that would show the call stack since the log will be in plain 
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
        private void AddRequestHeaders(HttpRequestSubmitter request, Func<string, string> tokenProcessor)
        {
            if (UseBasicAuthentication)
            {
                // .NET typically waits for a server challenge before sending authorization, so force the authorization headers 
                //  be sent rather than using a NetworkCredential object
                string authInfo = string.Format("{0}:{1}", Username, GetDecryptedPassword());
                string encodedAuthInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));

                request.Headers.Add("Authorization", "Basic " + encodedAuthInfo);
            }

            if (IncludeCustomHttpHeaders)
            {
                foreach (KeyValuePair<string, string> header in HttpHeaders)
                {
                    try
                    {
                        request.Headers.Add(header.Key, tokenProcessor(header.Value));
                    }
                    catch (ArgumentException ex)
                    {
                        log.Error("Error adding header in WebRequestTask", ex);
                        throw new ActionTaskRunException("Invalid header for WebRequest task.", ex);
                    }
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
            string message = string.Format("{0} {1} {2}{3}{2}{2}{4}", EnumHelper.GetDescription(request.Verb), request.Uri.AbsoluteUri, Environment.NewLine, headerText, postContent);

            requestLogger.LogRequest(message, "log");
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Invalid template output format</exception>
        private string GetContentType(TemplateOutputFormat templateOutputFormat)
        {
            switch (templateOutputFormat)
            {
                case TemplateOutputFormat.Html:
                    return "text/html";

                case TemplateOutputFormat.Xml:
                    return "text/xml";

                case TemplateOutputFormat.Text:
                    return (Verb == HttpVerb.Post) ? "application/x-www-form-urlencoded" : "";

                default:
                    throw new InvalidOperationException("Invalid template output format");
            }
        }
    }
}