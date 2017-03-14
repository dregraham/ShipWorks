using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using System.Runtime.Serialization;
using System.Globalization;
using System.Net;
using HtmlAgilityPack;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Exception for when a SOAP service returns an invalid response.  This looks for 
    /// html and tries to pull out the html body.
    /// </summary>
    [Serializable]
    public class InvalidSoapException : Exception
    {
        /// <summary>
        /// The response returned by the remote soap service
        /// </summary>
        string responseData = "";

        /// <summary>
        /// HttpWebResponse status code
        /// </summary>
        HttpStatusCode statusCode = HttpStatusCode.OK;

        /// <summary>
        /// HttpWebResponse status description
        /// </summary>
        string statusDescription = "";

        /// <summary>
        /// Response from the remote SOAP Service
        /// </summary>
        public string ResponseData
        {
            get { return responseData; }
        }

        /// <summary>
        /// Gets the Status code from the web response
        /// </summary>
        public HttpStatusCode HttpStatusCode
        {
            get { return statusCode; }
        }

        /// <summary>
        /// Returns the textual description of the HttpStatusCode
        /// </summary>
        public string StatusDescription
        {
            get { return statusDescription; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InvalidSoapException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InvalidSoapException(HttpStatusCode statusCode, string statusDescription, string responseText)
            : base(ReadWebError(statusCode, statusDescription, responseText))
        {
            this.responseData = responseText;
            this.statusCode = statusCode;
            this.statusDescription = statusDescription;
        }


        /// <summary>
        /// Deserialization Constructor
        /// </summary>
        protected InvalidSoapException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            responseData = info.GetString("ResponseData");
        }

        /// <summary>
        /// Serialization 
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("ResponseData", responseData);
        }

        /// <summary>
        /// Determines if this text provided can be considered an error string
        /// </summary>
        private static bool ContainsErrorText(string text)
        {
            if (Regex.IsMatch(text, "\b*error\b*", RegexOptions.IgnoreCase))
            {
                return true;
            }

            // other tests?

            return false;
        }

        /// <summary>
        /// Format a blob of text into decent English sentences.
        /// </summary>
        private static string FormatText(string text)
        {
            // first remove newlines
            text = Regex.Replace(text, @"(?<=\S)\n", " ", RegexOptions.Multiline);
            text = Regex.Replace(text, @"\n", "", RegexOptions.Multiline);

            // add blank lines between sentences, rudimentarily avoiding email addresses
            text = Regex.Replace(text, @"(?<!\w+\@\w+)\.\s*", ".\n\n");

            return text;
        }

        /// <summary>
        /// Attempts to extract an error message from the provided responseText.
        /// </summary>
        private static string ReadWebError(HttpStatusCode statusCode, string statusDescription, string responseText)
        {
            string title = "";
            string body = "";
            
            HtmlDocument agilityDoc = new HtmlDocument();
            agilityDoc.LoadHtml(responseText);

            HtmlNode titleNode = agilityDoc.DocumentNode.SelectSingleNode("/html/head/title");
            if (titleNode != null)
            {
                title = titleNode.InnerText;
                if (!ContainsErrorText(title))
                {
                    title = "";
                }
            }

            HtmlNode bodyNode = agilityDoc.DocumentNode.SelectSingleNode("/html/body");
            if (bodyNode != null)
            {
                body = bodyNode.InnerText;

                // format it
                body = FormatText(body);
            }

            string webError = title;
            if (webError.Length == 0)
            {
                // no good text was found, now try to fallback on a status code
                if (statusCode != HttpStatusCode.OK)
                {
                    webError = String.Format("ShipWorks received an invalid response from the server: {0}", statusDescription);
                }
                else
                {
                    webError = "ShipWorks received an invalid response from the server. ";
                }
            }

            // append the extracted body
            if (body.Length > 0)
            {
                if (!webError.EndsWith(".", StringComparison.OrdinalIgnoreCase))
                {
                    webError += ".";
                }

                webError += "\n" + body;
            }

            // cap the length
            if (webError.Length > 400)
            {
                webError = webError.Substring(0, 400) + "...";
            }

            return webError;
        }
    }
}
