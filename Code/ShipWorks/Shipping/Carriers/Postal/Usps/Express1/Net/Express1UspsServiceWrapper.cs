using System;
using System.Net;
using System.Reflection;
using System.Web.Services.Protocols;
using System.Xml;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v45;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net
{
    /// <summary>
    /// Custome web service proxy that enables ShipWorks to use Express1's web service via the USPS API 
    /// classes.
    /// </summary>
    public class Express1UspsServiceWrapper : SwsimV45  
    {
        // using relction, we need to set message.method.action, which is what message.Action is
        FieldInfo methodField;
        FieldInfo actionField;

        // the namespaces being swapped
        string expressNamespace = "http://www.express1.com/2011/08/ISDCV36Service";
        string uspsNamespace = "http://stamps.com/xml/namespace/2015/05/swsim/swsimv45";

        WebRequest webRequest = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsServiceWrapper(IApiLogEntry logEntry)
            : base(logEntry)
        {

        }

        /// <summary>
        /// Adjusts the Soap Action to reflect the new endpoint
        /// </summary>
        private void FixupOutgoingSoapMessage(SoapClientMessage message)
        {
            string newAction = message.Action;

            // Change the Outgoing message
            if (newAction.Contains(uspsNamespace))
            {
                newAction = expressNamespace + message.Action.Remove(0, uspsNamespace.Length);
            }

            // see if the action on the message needs to be changed
            if (string.Compare(newAction, message.Action, StringComparison.OrdinalIgnoreCase) != 0)
            {
                // use reflection to get access to the underlying object where we need to set the Action
                if (methodField == null)
                {
                    // using relction, we need to set message.method.action, which is what message.Action is
                    methodField = message.GetType().GetField("method", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (methodField == null)
                    {
                        throw new InvalidOperationException("Unable to get the method field");
                    }
                }

                object methodObject = methodField.GetValue(message);
                if (actionField == null)
                {
                    actionField = methodObject.GetType().GetField("action", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (actionField == null)
                    {
                        throw new InvalidOperationException("Unable to get the action field.");
                    }
                }

                actionField.SetValue(methodObject, newAction);
            }

            // Overwrite the SOAPAction header in the request every time, even if we didn't change the action.
            // This is due to a race condition relating to SoapClientMethod.action, which gets used as a prototype for
            // creating SoapClientMessages.  If there are multiple simultaneous Express1 calls made for the first time,
            // some will get sent with an invalid SOAPAction if we don't overwrite it every time.
            webRequest.Headers["SOAPAction"] = '"' + newAction + '"';
        }

        /// <summary>
        /// Creates the webrequest
        /// </summary>
        protected override WebRequest GetWebRequest(Uri uri)
        {
            // keep the webrequest so its headers can be changed as necessary
            return webRequest = base.GetWebRequest(uri);
        }

        /// <summary>
        /// Use a custom message reader so we can change what the incoming XML looks like to callers.
        /// </summary>
        protected override XmlReader GetReaderForMessage(System.Web.Services.Protocols.SoapClientMessage message, int bufferSize)
        {
            // return the custom XmlReader
            return new Express1UspsServiceResponseReader(base.GetReaderForMessage(message, bufferSize));
        }

        /// <summary>
        /// Use a custom message writer so we can change the outgoing XML
        /// </summary>
        protected override XmlWriter GetWriterForMessage(System.Web.Services.Protocols.SoapClientMessage message, int bufferSize)
        {
            // Manipulate the outgoing message 
            FixupOutgoingSoapMessage(message);

            // return the custome XmlWriter
            return new Express1UspsServiceRequestWriter(base.GetWriterForMessage(message, bufferSize));
        }
    }
}
