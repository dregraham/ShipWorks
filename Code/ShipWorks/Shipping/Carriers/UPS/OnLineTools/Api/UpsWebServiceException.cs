using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Services.Protocols;
using System.Xml;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Exception based on a SoapException thrown by UPS
    /// </summary>
    [Serializable]
    public class UpsWebServiceException : UpsException
    {
        private string severity = "";
        private string code = "";
        private string description = "";
        private string digest = "";

        /// <summary>
        /// Should only be used in Unit Tests...
        /// </summary>
        public UpsWebServiceException(string code)
        {
            this.code = code;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsWebServiceException(SoapException ex) :
            base(ex.Message, ex)
        {
            ExtractResult(ex.Detail);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected UpsWebServiceException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }

        /// <summary>
        /// Extract the results of the UPS exception from the given detail node
        /// </summary>
        private void ExtractResult(XmlNode xmlNode)
        {
            if (xmlNode.FirstChild != null && xmlNode.FirstChild.FirstChild != null)
            {
                var errorDetailNode = xmlNode.FirstChild.FirstChild;
                var primaryErrorNode = errorDetailNode.ChildNodes.OfType<XmlNode>().FirstOrDefault(n => n.LocalName == "PrimaryErrorCode");

                var severityNode = errorDetailNode.ChildNodes.OfType<XmlNode>().FirstOrDefault(n => n.LocalName == "Severity");
                severity = severityNode != null ? severityNode.InnerText : "";

                foreach (XmlNode node in primaryErrorNode.ChildNodes)
                {
                    switch (node.LocalName)
                    {
                        case "Code": code = node.InnerText; break;
                        case "Description": description = node.InnerText; break;
                        case "Digest": digest = node.InnerText; break;
                    }
                }

                if (!string.IsNullOrEmpty(description) && !description.EndsWith("."))
                {
                    description += ".";
                }
            }
        }

        /// <summary>
        /// Get the message to return to the user
        /// </summary>
        public override string Message
        {
            get
            {
                string message = description;

                if (!string.IsNullOrWhiteSpace(digest))
                {
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        message += " ";
                    }

                    message += digest;
                }

                if (!string.IsNullOrWhiteSpace(message))
                {
                    return message;
                }
                else
                {
                    return base.Message;
                }
            }
        }

        /// <summary>
        /// The SOAP error code
        /// </summary>
        public virtual string Code
        {
            get { return code; }
        }

        /// <summary>
        /// The UPS description of the error
        /// </summary>
        public string Description
        {
            get { return description; }
        }
    }
}
