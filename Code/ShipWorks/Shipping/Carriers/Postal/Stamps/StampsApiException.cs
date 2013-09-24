using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Globalization;
using System.Xml;
using System.Text.RegularExpressions;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Thrown when an SoapException is thrown while connecting to the stamps.com API
    /// </summary>
    public class StampsApiException : StampsException
    {
        long code;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsApiException(SoapException soapFault) :
            base(string.Empty, soapFault)
        {
            code = GetErrorCode(soapFault);
        }

        /// <summary>
        /// The user displayable exception message
        /// </summary>
        public override string Message
        {
            get
            {
                switch (code)
                {
                    case 0x002b0601: return "The username or password entered is not correct.";
                    case 0x80040414: return "There is not enough postage in your account.";
                }

                SoapException baseEx = base.InnerException as SoapException;
                if (baseEx.Message.ToUpperInvariant().Contains("UNABLE TO AUTHENTICATE"))
                {
                    // Handle the authentication exception from Express1 - there isn't any code to use with Express1;
                    // the actual error message itself is the best thing we have to go off of

                    // The exception message in the Express1 API ends up with portions of the stack trace, so show a friendlier 
                    // message to the user.
                    return "The username or password entered is not correct.";
                }

                string message;
                message = baseEx.Detail.InnerText;

                // Strip out the authenticator junk
                var authNode = baseEx.Detail.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => n.Name == "authenticator");
                if (authNode != null)
                {
                    message = message.Replace(authNode.InnerText, "");
                }

                if (string.IsNullOrEmpty(message))
                {
                    message = base.InnerException.Message;
                }

                message = message.Replace("Invalid SOAP message due to XML Schema validation failure.", "");
                message = message.Replace("http://stamps.com/xml/namespace/2008/01/swsim/swsimv1:", "");
                message = message.Replace("http://stamps.com/xml/namespace/2011/9/swsim/swsimv18:", "");
                message = message.Replace("http://stamps.com/xml/namespace/2011/11/swsim/swsimv20:", "");
            
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
        /// Get the Stamps.com API error code of the exception
        /// </summary>
        public override long Code
        {
            get
            {
                return code;
            }
        }

        /// <summary>
        /// Extract the numeric errror code from the Stamps.com exception
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
    }
}
