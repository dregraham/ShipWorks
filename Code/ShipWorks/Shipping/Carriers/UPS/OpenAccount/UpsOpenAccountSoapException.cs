using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    [Serializable]
    public class UpsOpenAccountSoapException : CarrierException
    {
        private string message;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOpenAccountSoapException(SoapException soapFault)
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
        /// Extract the numeric errror code from the USPS exception
        /// </summary>
        private void ParseException(SoapException ex)
        {
            message = "";

            if (ex.Detail != null && ex.Detail.FirstChild != null)
            {
                XElement faultNode = XElement.Parse(ex.Detail.FirstChild.OuterXml);

                // See if it is a SMART Pickup error
                XNamespace nameSpace = "http://www.ups.com/XMLSchema/XOLTWS/Error/v1.1";
                if (faultNode.Descendants(nameSpace + "Code").Any(code => code.Value == "9580091"))
                {
                    throw new UpsOpenAccountException("SMART Pickup Exception", UpsOpenAccountErrorCode.SmartPickupError);
                }

                XElement errorDescriptionElement = faultNode.Descendants(nameSpace + "Description").FirstOrDefault();
                if (errorDescriptionElement != null)
                {
                    message = errorDescriptionElement.Value;
                }
                else
                {
                    // Find the first message node
                    foreach (XElement messageNode in faultNode.Descendants().Where(e => e.Name.LocalName == "message"))
                    {
                        if (messageNode != null)
                        {
                            string text = (string)messageNode;

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