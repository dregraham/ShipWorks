﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Xml.XPath;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Templates.Processing;
using log4net;
using System.Text;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task to hit a URL.
    /// </summary>
    [ActionTask("Hit URL", "HitUrl")]
    public class HitUrlTask : TemplateBasedTask
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(HitUrlTask));

        /// <summary>
        /// Gets or sets the verb.
        /// </summary>
        public HttpVerb Verb { get; set; }

        /// <summary>
        /// Gets or sets the URL to hit.
        /// </summary>
        public string UrlToHit { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to [use basic authentication].
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
        /// Create the editor for the settings
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new HitUrlTaskEditor(this);
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
        /// Sends template to URL (or just sends request if no Post Data)
        /// </summary>
        protected override void ProcessTemplateResults(TemplateEntity template, IList<TemplateResult> templateResults, ActionStepContext context)
        {
            foreach (TemplateResult templateResult in templateResults)
            {
                HttpTextPostRequestSubmitter request = new HttpTextPostRequestSubmitter(templateResult.ReadResult(), GetContentType((TemplateOutputFormat) template.OutputFormat));
                ProcessRequest(request, new List<long>());
            }
        }

        /// <summary>
        /// Run the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            // Make note of the keys being used, so we can use them when processing the request. Not
            // passing these as a parameter to the method because the 

            if (TemplateID == 0)
            {
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                ProcessRequest(request, inputKeys);
            }
            else
            {
                base.Run();                
            }
        }


        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="request">The request being submitted.</param>
        /// <param name="inputKeys">The input keys used for processing tokens in the URL.</param>
        /// <exception cref="ActionTaskRunException">
        /// Invalid header for HitUrl task.
        /// or
        /// Url not formatted properly.
        /// or
        /// Error hitting URL.
        /// </exception>
        private void ProcessRequest(HttpRequestSubmitter request, List<long> inputKeys)
        {
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.HitUrlTask, "HitUrlTask");
            
            try
            {
                string processedUrl = TemplateTokenProcessor.ProcessTokens(UrlToHit, inputKeys);
                request.Uri = new Uri(processedUrl);

                request.Verb = Verb;
                AddAuthenticationHeader(request);

                foreach (KeyValuePair<string, string> header in HttpHeaders)
                {
                    try
                    {
                        request.Headers.Add(HttpUtility.UrlDecode(header.Key), HttpUtility.UrlDecode(header.Value));
                    }
                    catch (ArgumentException ex)
                    {
                        log.Error("Error adding header in HitURLTask", ex);
                        throw new ActionTaskRunException("Invalid header for HitUrl task.", ex);
                    }
                }

                logger.LogRequest(request);

                IHttpResponseReader httpResponseReader = request.GetResponse();
                logger.LogResponse(httpResponseReader.ReadResult(), "txt");
            }
            catch (UriFormatException ex)
            {
                log.Error("Error in HitUrl Url format.", ex);
                throw new ActionTaskRunException("Url not formatted properly.", ex);
            }
            catch (WebException ex)
            {
                logger.LogResponse(ex);
                throw new ActionTaskRunException("Error hitting URL.", ex);
            }
        }

        /// <summary>
        /// Adds the authentication header to the request if needed.
        /// </summary>
        /// <param name="request">The request.</param>
        private void AddAuthenticationHeader(HttpRequestSubmitter request)
        {
            if (UseBasicAuthentication)
            {
                // .NET typically waits for a server challenge before sending authorization, so force the authorization headers 
                //  be sent rather than using a NetworkCredential object
                string authInfo = string.Format("{0}:{1}", Username, Password);
                string encodedAuthInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                request.Headers.Add("Authorization", "Basic " + encodedAuthInfo);
            }
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