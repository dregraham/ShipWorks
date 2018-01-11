using System;
using System.Net;
using System.Reflection;
using System.Web.Services.Protocols;
using System.Xml;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.OpenShip
{
    /// <summary>
    /// Wraps FedExService to use OpenShip.
    /// </summary>
    public class FedExShipServiceWrapper : ShipService
    {
        // using reflection, we need to set message.method.action, which is what message.Action is
        FieldInfo methodField;
        FieldInfo actionField;

        private readonly string shipNamespace = string.Format("http://fedex.com/ws/ship/v{0}", FedExSettings.ShipVersionNumber);
        private readonly string openShipNamespace = string.Format("http://fedex.com/ws/openship/v{0}", FedExSettings.OpenShipVersionNumber);

        WebRequest webRequest = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipServiceWrapper(IApiLogEntry logEntry)
            : base(logEntry)
        {
            
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            return new FedExOpenShipXmlReader(base.GetReaderForMessage(message, bufferSize));
        }

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            // Manipulate the outgoing message 
            FixupOutgoingSoapMessage(message);

            return new FedExOpenShipXmlWriter(base.GetWriterForMessage(message,bufferSize));
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
        /// Fixup the outgoing SOAP message. Ensures action is correct.
        /// </summary>
        private void FixupOutgoingSoapMessage(SoapClientMessage message)
        {
            string newAction = message.Action;

            // Change the Outgoing message
            if (newAction.Contains(shipNamespace))
            {
                newAction = openShipNamespace + message.Action.Remove(0, shipNamespace.Length);
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
    }
}
