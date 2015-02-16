using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    [Serializable]
    public class FedExSoapCarrierException : CarrierException
    {
        private string message;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExSoapCarrierException(SoapException soapFault)
            :
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
        /// Extract the numeric error code from the FedEx exception
        /// </summary>
        public void ParseException(SoapException ex)
        {
            message = "";

            if (ex.Detail != null && ex.Detail.FirstChild != null)
            {
                XElement faultNode = XElement.Parse(ex.Detail.FirstChild.OuterXml);

                // Find the first message node
                foreach (XElement messageNode in faultNode.Descendants().Where(e => e.Name.LocalName == "message"))
                {
                    if (messageNode != null)
                    {
                        string text = (string) messageNode;

                        text = Regex.Replace(text, "@.*?'", "'", RegexOptions.Singleline);
                        text = Regex.Replace(text, "@.*?$", "", RegexOptions.Singleline);

                        text = text.Trim();
                        if (!text.EndsWith(".", StringComparison.InvariantCulture))
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
                if (!message.EndsWith(".", StringComparison.InvariantCulture))
                {
                    message += ".";
                }
            }
        }
    }
}
