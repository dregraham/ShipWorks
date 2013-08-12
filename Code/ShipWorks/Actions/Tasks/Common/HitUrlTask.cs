﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.XPath;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Templates.Processing;
using log4net;

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
        public HttpVerb Verb
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL to hit.
        /// </summary>
        public string UrlToHit
        {
            get;
            set;
        }


        /// <summary>
        /// The object is being deserialized into its values
        /// </summary>
        public override void DeserializeXml(XPathNavigator xpath)
        {
            base.DeserializeXml(xpath);

            XPathNodeIterator headeriIterator = xpath.Select("/Settings/HttpHeaders/*/@value");

            List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
            foreach (XPathNavigator header in headeriIterator)
            {
                string[] keyValue = ((string)header.TypedValue).Split(',');

                // ignore the first character as it is "["
                string key = keyValue[0].Substring(1).Trim();

                // ignore the last character as it is "]"
                string value = keyValue[1].Substring(0, keyValue[1].Length - 1).Trim();

                headerList.Add(new KeyValuePair<string, string>(key,value));
            }

            HttpHeaders = headerList.ToArray();
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
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP headers.
        /// </summary>
        public KeyValuePair<string,string>[] HttpHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether to postpone running or not.
        /// </summary>
        public override bool EnablePostpone
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Enter Webpage information you want to call.";
            }
        }

        /// <summary>
        /// Create the editor for the settings
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new HitUrlTaskEditor(this);
        }

        /// <summary>
        /// Sends template to URL (or just sends request if no Post Data)
        /// </summary>
        protected override void ProcessTemplateResults(TemplateEntity template, IList<TemplateResult> templateResults, ActionStepContext context)
        {
            if (Verb == HttpVerb.Get)
            {
                HttpRequestSubmitter request = new HttpTextPostRequestSubmitter("", GetContentType(TemplateOutputFormat.Text));
                ProcessResult(request);
            }
            else
            {
                foreach (var templateResult in templateResults)
                {
                    HttpTextPostRequestSubmitter request = new HttpTextPostRequestSubmitter(templateResult.ReadResult(), GetContentType((TemplateOutputFormat) template.OutputFormat));
                    ProcessResult(request);
                }
            }
        }

        /// <summary>
        /// Processes the result.
        /// </summary>
        private void ProcessResult(HttpRequestSubmitter request)
        {
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.HitUrlTask, "HitUrlTask");

            request.Uri = new Uri(UrlToHit);
            request.Verb = Verb;

            if (UseBasicAuthentication)
            {
                request.Credentials = new NetworkCredential(Username, Password);
            }

            foreach (KeyValuePair<string, string> header in HttpHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            logger.LogRequest(request);

            IHttpResponseReader httpResponseReader = request.GetResponse();

            logger.LogResponse(httpResponseReader.ReadResult(), "txt");
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private static string GetContentType(TemplateOutputFormat templateOutputFormat)
        {
            switch (templateOutputFormat)
            {
                case TemplateOutputFormat.Html:
                    return "text/html";
                case TemplateOutputFormat.Xml:
                    return "text/xml";
                case TemplateOutputFormat.Text:
                    return "text/plain";
                default:
                    throw new ArgumentOutOfRangeException("templateOutputFormat", "Invalid template Output Format");
            }
        }
    }
}