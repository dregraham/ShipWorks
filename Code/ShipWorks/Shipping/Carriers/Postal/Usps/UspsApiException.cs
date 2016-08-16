using System;
using System.Linq;
using System.Web.Services.Protocols;
using System.Globalization;
using System.Xml;
using System.Text.RegularExpressions;
using Interapptive.Shared;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Thrown when an SoapException is thrown while connecting to the USPS API
    /// </summary>
    public class UspsApiException : UspsException
    {
        long code;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsApiException(SoapException soapFault) :
            base(string.Empty, soapFault)
        {
            code = GetErrorCode(soapFault);
        }

        /// <summary>
        /// The user displayable exception message
        /// </summary>
        public override string Message
        {
            [NDependIgnoreLongMethod]
            get
            {
                switch (code)
                {
                    // Errors coming from the Express1 version of the API (prior to hitting USPS) always
                    // return an error code of 0
                    case 0x00000000: return GetExpress1Message();

                    case 0x002b0601: return "The username or password entered is not correct.";
                    case 0x80040414: return "There is not enough postage in your account.";
                    case 0x005f0302: return "This account is already a reseller account.";
                    case 0x005f0301: return string.Format("This account is a multi-user account. Multi-user accounts are not eligible to be migrated to a reseller account.{0}{0}Please contact Stamps.com if you need assistance.", Environment.NewLine);
                }

                SoapException baseEx = InnerException as SoapException;
                string message = baseEx.Detail.InnerText;

                // Strip out the authenticator junk
                var authNode = baseEx.Detail.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => n.Name == "authenticator");
                if (authNode != null)
                {
                    message = message.Replace(authNode.InnerText, "");
                }

                if (string.IsNullOrEmpty(message))
                {
                    message = InnerException.Message;
                }

                message = message.Replace("Invalid SOAP message due to XML Schema validation failure.", "");
                message = RemoveNamespaceFromMessage(message);

                string leftOver = string.Empty;

                int swsIndex = message.IndexOf(" SWS");
                if (swsIndex > 0)
                {
                    // Go to the first period (as long as there isn't a message in parens to follow)
                    int stopIndex = message.IndexOf(".", swsIndex);
                    if (stopIndex >= 0 && (message.IndexOf("(", swsIndex) == -1 || message.IndexOf(")", swsIndex) == -1))
                    {
                        message = message.Remove(swsIndex, (stopIndex - swsIndex) + 1);
                    }
                    else
                    {
                        leftOver = message.Substring(swsIndex);
                        message = message.Substring(0, swsIndex);
                    }
                }

                if (!string.IsNullOrEmpty(message) && !message.EndsWith("."))
                {
                    message += ".";
                }

                // Sometimes extra info follows in parens
                Match match = Regex.Match(leftOver, @"\(([^)]+)");
                if (match.Success)
                {
                    if (message.Length > 0)
                    {
                        message += " ";
                    }

                    message += match.Groups[1].Value;
                }

                return message.Trim();
            }
        }

        /// <summary>
        /// Remove the namespace from the message.
        /// </summary>
        private static string RemoveNamespaceFromMessage(string message)
        {
            SoapDocumentMethodAttribute soapDocumentMethodAttribute = UspsWebClient.WebServiceType.
                GetMethod("AuthenticateUser").
                GetCustomAttributes(true).
                OfType<SoapDocumentMethodAttribute>().
                FirstOrDefault();

            if (soapDocumentMethodAttribute != null)
            {
                message = message.Replace(soapDocumentMethodAttribute.ResponseNamespace + ":", string.Empty);
            }

            return message;
        }

        /// <summary>
        /// Get the USPS API error code of the exception
        /// </summary>
        public override long Code
        {
            get
            {
                return code;
            }
        }

        /// <summary>
        /// Extract the numeric error code from the USPS exception
        /// </summary>
        public static long GetErrorCode(SoapException ex)
        {
            long code = 0;

            if (ex.Detail != null && ex.Detail.FirstChild != null)
            {
                XmlAttribute codeAttribute = ex.Detail.FirstChild.Attributes["code"];

                if (codeAttribute != null)
                {
                    if (long.TryParse(codeAttribute.Value, NumberStyles.HexNumber, null, out code))
                    {
                        return code;
                    }
                }
            }

            return code;
        }

        /// <summary>
        /// Extracts a message that is specific to the Express1 version of the USPS API (i.e. when the
        /// error code in the SoapException is 0).
        /// </summary>
        private string GetExpress1Message()
        {
            SoapException soapException = InnerException as SoapException;

            // Just a generic message if the inner exception is not a SoapException for some reason
            string message = "An error occurred communicating with Express1";

            if (soapException != null)
            {
                if (soapException.Message.ToUpperInvariant().Contains("UNABLE TO AUTHENTICATE"))
                {
                    // Use a more descriptive error message
                    message = "The username or password entered is not correct.";
                }
                else
                {
                    // Errors coming from the Express1 version of the API (prior to hitting USPS) have a decent error
                    // message, so we can just return the exception message
                    message = soapException.Message;
                }
            }

            return message;
        }
    }
}
