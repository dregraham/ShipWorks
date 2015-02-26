using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Thrown when an exception occurrs while access the FedEx API
    /// </summary>
    public class FedExSoapException : FedExException
    {
        long code;
        string message;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExSoapException(SoapException soapFault) :
            base(string.Empty, soapFault)
        {
            ParseException(soapFault);
        }

        /// <summary>
        /// The user displayable exception message
        /// </summary>
        public override string Message
        {
            get
            {
                return message;
            }
        }

        /// <summary>
        /// Get the FedEx API error code of the exception
        /// </summary>
        public override long Code
        {
            get
            {
                return code;
            }
        }

        /// <summary>
        /// Extract the numeric errror code from FedEx exception
        /// </summary>
        public void ParseException(SoapException ex)
        {
            code = 0;
            message = "";

            if (ex.Detail != null && ex.Detail.FirstChild != null)
            {
                XElement faultNode = XElement.Parse(ex.Detail.FirstChild.OuterXml);

                XElement errorCodeNode = faultNode.Elements().FirstOrDefault(e => e.Name.LocalName == "errorCode");
                if (errorCodeNode != null)
                {
                    code = (long) errorCodeNode;
                }

                // Find the first message node
                foreach (XElement messageNode in faultNode.Descendants().Where(e => e.Name.LocalName == "message"))
                {
                    if (messageNode != null)
                    {
                        string text = (string) messageNode;

                        text = Regex.Replace(text, "@.*?'", "'", RegexOptions.Singleline);
                        text = Regex.Replace(text, "@.*?$", "", RegexOptions.Singleline);

                        text = text.Trim();
                        if (!text.EndsWith("."))
                        {
                            text += ".";
                        }

                        if (message.Length > 0)
                        {
                            message += "\n";
                        }

                        message += text;
                    }
                }
            }

            // If its blank, fallback to the exception message
            if (string.IsNullOrEmpty(message))
            {
                message = ex.Message;

                message = message.Trim();
                if (!message.EndsWith("."))
                {
                    message += ".";
                }
            }
        }
    }
}
